using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using POLYGLOT.Project.Security.application.Dto;
using POLYGLOT.Project.Security.application.Exceptions;
using POLYGLOT.Project.Security.application.Interfaces;
using POLYGLOT.Project.Security.application.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POLYGLOT.Project.Security.infraestructure.Repositories
{
    public class GetTokenRepository : IGetToken
    {

        private readonly DbSecurityContext _context;
        private readonly IOptions<JwtSettings> _options;
        public GetTokenRepository(DbSecurityContext context, IOptions<JwtSettings> options)
        {
            _context = context;
            _options = options;
        }

        public async Task<AuthResponse> ObtenerToken(AuthRequest request)
        {
            try
            {
                var access = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.Username && x.Password == request.Password) ?? throw new BaseCustomException("Credenciales Incorrectas", "", 404);

                var token = GenerarToken(access);

                var res = new AuthResponse()
                {
                    Token = token,
                    Expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_options.Value.Expiration))
                };

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GenerarToken(User usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Username)
            };

            var token = new JwtSecurityToken(
                _options.Value.Issuer,
                _options.Value.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_options.Value.Expiration)),
                signingCredentials: creds
                );
            string _token = new JwtSecurityTokenHandler().WriteToken(token);

            return _token;
        }
    }
}
