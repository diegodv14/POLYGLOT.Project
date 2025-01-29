using Microsoft.Extensions.Configuration;
using POLYGLOT.Project.Pay.application.Dto;
using POLYGLOT.Project.Pay.application.Interfaces;

namespace POLYGLOT.Project.Pay.infraestructure.Repositories
{
    public class PayInvoiceRepository : IPayInvoice
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public PayInvoiceRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> HandlePayInvoice(PayInvoiceRequest request)
        {
            try
            {
                var urlVerifyInvocice = _configuration.GetConnectionString("CheckInvoiceApi") + $"?idInvoice={request.IdInvoice}";
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
