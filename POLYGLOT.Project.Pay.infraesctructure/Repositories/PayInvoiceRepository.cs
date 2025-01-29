using POLYGLOT.Project.Pay.application.Interfaces;

namespace POLYGLOT.Project.Pay.infraestructure.Repositories
{
    public class PayInvoiceRepository : IPayInvoice
    {
        private readonly HttpClient _httpClient;
        public PayInvoiceRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> HandlePayInvoice()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
