using POLYGLOT.Project.Pay.application.Dto;

namespace POLYGLOT.Project.Pay.application.Interfaces
{
    public interface IPayInvoice
    {
        Task<ResponseSuccess<object>> Execute(PayInvoiceRequest payInvoice, string? token);
    }
}
