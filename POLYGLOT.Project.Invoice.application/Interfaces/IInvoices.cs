using POLYGLOT.Project.Invoice.application.Dto;

namespace POLYGLOT.Project.Invoice.application.Interfaces
{
    public interface IInvoices
    {
        Task<InvoiceResponse> GetInvoices();
        Task<ResponseSuccess> AddInvoice(AddInvoiceDto data);
        Task<POLYGLOT.Project.Invoice.application.Models.Invoice> CheckInvoice(int secuencial);
    }
}
