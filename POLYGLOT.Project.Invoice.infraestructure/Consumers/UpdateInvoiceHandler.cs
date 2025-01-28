using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace POLYGLOT.Project.Invoice.infraestructure.Consumers
{
    public class UpdateInvoiceHandler : BackgroundService
    {

        private readonly IConnectionFactory _factory;
        private readonly IConfiguration _configuration;
        public UpdateInvoiceHandler(IConnectionFactory factory, IConfiguration configuration) 
        {
            _factory = factory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IConnection? connection = null;
                IChannel? channel = null;

                try
                {
                    connection = await _factory.CreateConnectionAsync(stoppingToken);
                    channel = await connection.CreateChannelAsync();

                    string queueName = _configuration["RabbitMQ:Queue"]
                        ?? throw new Exception("La cola de RabbitMQ no está definida.");

                    await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);

                    string exchangeName = _configuration["RabbitMQ:Exchange"]
                        ?? throw new Exception("El exchange no está definido.");
                    string routingKey = _configuration["RabbitMQ:RoutingKey"]
                        ?? throw new Exception("La routingKey no está definida.");

                    await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey, cancellationToken: stoppingToken);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

                    Console.WriteLine("Empezando a escuchar cola de RabbitMQ.......... \n");
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        Console.WriteLine("Mensaje Recibido de RabbitMQ");
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        try
                        {
                            var res = JsonSerializer.Deserialize<POLYGLOT.Project.Invoice.application.Models.Invoice>(message);
                            Console.WriteLine(res);
                            //await _invoices.UpdateInvoice(res!.IdInvoice);
                            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error procesando mensaje {message}: {ex.Message}");
                            await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
                        }
                    };

                    await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al intentar conectarse a RabbitMQ o iniciar el consumidor: {ex.Message}");
                }
                finally
                {
                    channel?.Dispose();
                    connection?.Dispose();
                }

                await Task.Delay(5000, stoppingToken);
            }
        }

    }
}
