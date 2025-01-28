using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using POLYGLOT.Project.Invoice.application.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace POLYGLOT.Project.Invoice.infraestructure.Handlers
{
    public class UpdateInvoiceHandler : BackgroundService
    {

        private readonly IInvoices _invoices;
        private readonly IConnectionFactory _factory;
        private readonly IConfiguration _configuration;
        public UpdateInvoiceHandler(IInvoices invoices, IConnectionFactory connection, IConfiguration configuration) 
        { 
           _invoices = invoices;
           _factory = connection;
           _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            try
            {
                var _connection = await _factory.CreateConnectionAsync();
                var _channel = await _connection.CreateChannelAsync();

                string queueName = _configuration.GetConnectionString("RabbitMQ:Queue") ?? throw new Exception("La cola de RabbitMQ no esta definida.");

                await _channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);


                string exchangeName = _configuration.GetConnectionString("RabbitMQ:Exchange") ?? throw new Exception("El exchange no esta definido.");
                string routingKey = _configuration.GetConnectionString("RabbitMQ:RoutingKey") ?? throw new Exception("La routingKey no esta definida.");

                await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

                var consumer = new AsyncEventingBasicConsumer(_channel);

                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

                Console.WriteLine("Empezando a escuchar cola de RabbitMQ.......... \n");
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        await _channel.BasicCancelAsync(consumer.ConsumerTags[0]);
                        return;
                    }
                    Console.WriteLine("Mensaje Recibido de RabbitMQ");
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    try
                    {
                        var res = JsonSerializer.Deserialize<POLYGLOT.Project.Invoice.application.Models.Invoice>(message);
                        await _invoices.UpdateInvoice(res!.IdInvoice);
                        await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error procesando mensaje {message}: {ex.Message}");
                        await _channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                    }
                };

                await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar conectarse a RabbitMQ o iniciar el consumidor: {ex.Message}");
            
            }

            await Task.CompletedTask;

        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
