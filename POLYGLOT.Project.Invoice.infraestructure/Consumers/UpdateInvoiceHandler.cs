using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POLYGLOT.Project.Invoice.application.Dto;
using POLYGLOT.Project.Invoice.application.Exceptions;
using POLYGLOT.Project.Invoice.application.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace POLYGLOT.Project.Invoice.infraestructure.Consumers
{
    public class UpdateInvoiceHandler : BackgroundService
    {
        private readonly IConnectionFactory _factory;
        private readonly IServiceProvider _service;
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IChannel? _channel;

        public UpdateInvoiceHandler(IConnectionFactory factory, IConfiguration configuration, IServiceProvider service)
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
                _channel = await _connection.CreateChannelAsync();

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

                await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

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
                var res = JsonSerializer.Deserialize<ConsumeInvoiceDto>(message)
                    ?? throw new Exception("El mensaje recibido es nulo.");

                using var scope = _service.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DbInvoiceContext>();

                Console.WriteLine("📩 Mensaje recibido de RabbitMQ...");
                await UpdateInvoice(res, dbContext);

                await _channel!.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error procesando mensaje: {ex.Message}");
            }
        }

        private static async Task<bool> UpdateInvoice(ConsumeInvoiceDto res, DbInvoiceContext _context)
        {
            try
            {
                var invoice = await _context.Invoices.FirstOrDefaultAsync(s => s.IdInvoice == res.idInvoice)
                    ?? throw new BaseCustomException($"La Factura con id {res.idInvoice} no existe", "", 404);

                invoice.Paid += res.paid;

                if (invoice.Amount == invoice.Paid)
                {
                    invoice.State = true;
                }

                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error actualizando la factura: {ex.Message}");
                return false;
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("⚠️ Cerrando conexión con RabbitMQ...");
            _channel?.Dispose();
            _connection?.Dispose();
            await base.StopAsync(cancellationToken);
        }
    }
}
