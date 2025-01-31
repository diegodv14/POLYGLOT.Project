using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using POLYGLOT.Project.Transaction.application.Dto;
using MongoDB.Driver;
using POLYGLOT.Project.Transaction.application.Models;

namespace POLYGLOT.Project.Transaction.infraestructure.Consumer
{
    public class AddTransaccionHandlerConsumer : BackgroundService
    {
        private readonly IConnectionFactory _factory;
        private readonly IServiceProvider _service;
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;

        public AddTransaccionHandlerConsumer(IConnectionFactory factory, IConfiguration configuration, IServiceProvider service)
        {
            _factory = factory;
            _configuration = configuration;
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _connection = await _factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                string queueName = _configuration["RabbitMQ:Queue"]
                    ?? throw new Exception("La cola de RabbitMQ no está definida.");

                string exchangeName = _configuration["RabbitMQ:Exchange"]
                    ?? throw new Exception("El exchange no está definido.");

                string routingKey = _configuration["RabbitMQ:RoutingKey"]
                    ?? throw new Exception("La routingKey no está definida.");

                await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey, cancellationToken: stoppingToken);
                await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: stoppingToken);

                Console.WriteLine("✅ Consumidor RabbitMQ iniciado...");

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) => await ProcessMessage(ea);

                await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(5000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al iniciar RabbitMQ: {ex.Message}");
            }
        }

        private async Task ProcessMessage(BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var res = JsonSerializer.Deserialize<ConsumerTransaccionDto>(message)
                    ?? throw new Exception("El mensaje recibido es nulo.");

                using var scope = _service.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IMongoDatabase>();

                Console.WriteLine("📩 Mensaje recibido de RabbitMQ...");
                await SaveTransaccion(res, dbContext);

                await _channel!.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error procesando mensaje: {ex.Message}");
            }
        }


        private async Task<bool> SaveTransaccion(ConsumerTransaccionDto res, IMongoDatabase _context)
        {
            try
            {
                var collection = _context.GetCollection<TransactionModel>(_configuration["MongoSettings:Collection"]);

                await collection.InsertOneAsync(new TransactionModel()
                {
                    IdInvoice = res.IdInvoice,
                    Amount = res.Amount,
                    Date = res.Date
                });

                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
