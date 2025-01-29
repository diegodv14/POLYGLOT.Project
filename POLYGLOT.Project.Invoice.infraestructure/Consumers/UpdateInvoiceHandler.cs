﻿using Microsoft.EntityFrameworkCore;
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
        private readonly IServiceProvider __service;
        private readonly IConfiguration _configuration;
        public UpdateInvoiceHandler(IConnectionFactory factory, IConfiguration configuration, IServiceProvider service) 
        {
            _factory = factory;
            _configuration = configuration;
            __service = service;
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

                    await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    string exchangeName = _configuration["RabbitMQ:Exchange"]
                        ?? throw new Exception("El exchange no está definido.");
                    string routingKey = _configuration["RabbitMQ:RoutingKey"]
                        ?? throw new Exception("La routingKey no está definida.");

                    await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);

                    var consumer = new AsyncEventingBasicConsumer(channel);

                    await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

                    Console.WriteLine("Empezando a escuchar cola de RabbitMQ.......... \n");
                    consumer.ReceivedAsync += async (model, ea) =>
                    {

                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        try
                        {
                            var res = JsonSerializer.Deserialize<ConsumeInvoiceDto>(message);

                            using (var scope = __service.CreateScope())
                            {
                                Console.WriteLine("Mensaje Recibido de RabbitMQ....");
                                var dbContext = scope.ServiceProvider.GetRequiredService<DbInvoiceContext>();

                                await UpdateInvoice(res, dbContext);
                            }

                            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error procesando mensaje {message}: {ex.Message}");
                        }
                    };

                    await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
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

        private async Task<bool> UpdateInvoice(ConsumeInvoiceDto res, DbInvoiceContext _context)
        {
            try 
            {
                var invoice = await _context.Invoices.FirstOrDefaultAsync(s => s.IdInvoice == res.idInvoice) ?? throw new BaseCustomException($"La Factura con id {res.idInvoice} no existe", "", 404);

                invoice.Paid += res.paid;

                if(invoice.Amount == invoice.Paid)
                {
                    invoice.State = true;
                }
                _context.Invoices.Update(invoice);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
