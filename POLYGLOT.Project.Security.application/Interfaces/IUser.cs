using POLYGLOT.Project.Security.application.Dto;

namespace POLYGLOT.Project.Security.application.Interfaces
{
    public interface IUser
    {
        Task<ResponseSuccess> AddUser(AuthRequest data);
    }
}
