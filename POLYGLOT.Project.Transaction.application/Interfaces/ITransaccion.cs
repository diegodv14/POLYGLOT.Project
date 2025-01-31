using POLYGLOT.Project.Transaction.application.Dto;

namespace POLYGLOT.Project.Transaction.application.Interfaces
{
    public interface ITransaccion
    {
        Task<GetTransaccionResponse> GetTransaccionList();
    }
}
