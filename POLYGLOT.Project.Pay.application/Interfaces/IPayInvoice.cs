namespace POLYGLOT.Project.Pay.application.Interfaces
{
    public interface IPayInvoice
    {
        Task<bool> HandlePayInvoice();
    }
}
