using POLYGLOT.Project.Invoice.application.Dto;

namespace POLYGLOT.Project.Invoice.application.Interfaces
{
    public interface IInvoices
    {
        Task<InvoiceResponse> GetInvoices();
        Task<bool> UpdateInvoice(int idInvoice);
    }
}
