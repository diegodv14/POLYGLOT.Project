using Microsoft.Extensions.Configuration;
using POLYGLOT.Project.Pay.application.Dto;
using POLYGLOT.Project.Pay.application.Interfaces;
using POLYGLOT.Project.Pay.application.Exceptions;
using System.Text.Json;
using POLYGLOT.Project.Pay.application.Models;

namespace POLYGLOT.Project.Pay.infraestructure.Repositories
{
    public class PayInvoiceRepository : IPayInvoice
    {
        private readonly HttpClient _http;
        private readonly IRabbitMQ _rabbitmq;
        private readonly DbOperationContext _context;
        private readonly IConfiguration _configuration;
        public PayInvoiceRepository(HttpClient httpClient, IConfiguration configuration, DbOperationContext context, IRabbitMQ rabbitmq)
        {
            _http = httpClient;
            _configuration = configuration;
            _context = context;
            _rabbitmq = rabbitmq;
        }

        public async Task<ResponseSuccess<object>> Execute(PayInvoiceRequest request, string? token)
        {

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {

                var urlVerifyInvocice = _configuration["cn:checkInvoiceApi"] + $"?secuencial={request.Secuencial}";

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, urlVerifyInvocice)
                {
                    Headers =
                    {
                        { "Authorization", $"Bearer {token}" }
                    }
                };

                var res = await _http.SendAsync(requestMessage);

                if (!res.IsSuccessStatusCode)
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    var error = JsonSerializer.Deserialize<ResponseError>(errorContent);
                    throw new BaseCustomException($"{error!.Message}", "", error.Code);
                }

                var content = await res.Content.ReadAsStringAsync();
                var invoice = JsonSerializer.Deserialize<InvoiceDto>(content);

                if (invoice.amount == invoice.paid) throw new BaseCustomException($"La deuda de esta factura ya se encuentra pagada.", "", 409);

                if ((request.Amount + invoice!.paid) > invoice.amount) throw new BaseCustomException($"La deuda de esta factura es de {invoice.amount - invoice.paid}. Puede terminar de cancelarla completamente o pagar una parte","", 409);

                var newPaid = new Operation()
                {
                    IdInvoice = invoice.idInvoice,
                    Amount = request.Amount,
                    Date = DateTime.Now
                };

                _context.Operations.Add(newPaid);
                var saveResult = await _context.SaveChangesAsync();

                if (saveResult <= 0)
                {
                    throw new BaseCustomException("No se pudo guardar el pago en la base de datos", "", 500);
                }

                var invoiceInfo = new InvoiceDto()
                {
                    idInvoice = invoice.idInvoice,
                    amount = invoice.amount,
                    paid = request.Amount,
                    secuencial = invoice.secuencial,
                    state = invoice.state
                };

                var transactionInfo = new TransaccionDto()
                {
                    IdInvoice = invoice.idInvoice,
                    Amount = request.Amount,
                    Date = DateTime.Now
                };

                var exchange = _configuration["RabbitMQ:Exchange"];
                var routingKeyInvoice = _configuration["RabbitMQ:RoutingKeyInvoice"];
                var routingKeyTransaccion = _configuration["RabbitMQ:RoutingKeyTransaccion"];

                if (string.IsNullOrEmpty(exchange) || string.IsNullOrEmpty(routingKeyInvoice) || string.IsNullOrEmpty(routingKeyTransaccion))
                {
                    throw new BaseCustomException("Configuración de RabbitMQ incompleta", "", 500);
                }

                var invoicePublish = _rabbitmq.PublishQueue(invoiceInfo, exchange, routingKeyInvoice);
                var transactionPublish = _rabbitmq.PublishQueue(transactionInfo, exchange, routingKeyTransaccion);

                await Task.WhenAll(invoicePublish, transactionPublish);

                await transaction.CommitAsync();

                return new ResponseSuccess<object>()
                {
                    Message = "Pago registrado exitosamente",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
    }
}
