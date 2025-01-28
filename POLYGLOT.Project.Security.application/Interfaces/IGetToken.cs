using POLYGLOT.Project.Security.application.Models;

namespace POLYGLOT.Project.Security.application.Interfaces
{
    public interface IGetToken
    {
        Task<AuthResponse> ObtenerToken(AuthRequest request);
    }
}
