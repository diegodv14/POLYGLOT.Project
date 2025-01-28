namespace POLYGLOT.Project.Invoice.application.Interfaces
{
    public interface IInvoices
    {
        Task<dynamic> GetInvoices();
        Task<bool> UpdateInvoice(int idInvoice);
    }
}
