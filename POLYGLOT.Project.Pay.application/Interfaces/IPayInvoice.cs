using POLYGLOT.Project.Pay.application.Dto;

namespace POLYGLOT.Project.Pay.application.Interfaces
{
    public interface IPayInvoice
    {
        Task<bool> HandlePayInvoice(PayInvoiceRequest request);
    }
}
