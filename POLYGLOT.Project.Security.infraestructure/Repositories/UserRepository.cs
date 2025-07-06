using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using POLYGLOT.Project.Security.application.Dto;
using POLYGLOT.Project.Security.application.Exceptions;
using POLYGLOT.Project.Security.application.Interfaces;
using POLYGLOT.Project.Security.application.Models;

namespace POLYGLOT.Project.Security.infraestructure.Repositories
{
    public class UserRepository : IUser
    {

        private readonly DbSecurityContext _context;
        public UserRepository(DbSecurityContext context) 
        {
            _context = context;
        }

        public async Task<ResponseSuccess> AddUser(AuthRequest data)
        {
            try
            {   
                var userExists = await _context.Users.FirstOrDefaultAsync(s => s.Username == data.Username);

                if (userExists != null)
                    throw new BaseCustomException("Ya existe un usuario registrado con ese nombre.", "", 409);


                var passwordHasher = new PasswordHasher<User>();

                var hashedPassword = passwordHasher.HashPassword(null, data.Password);

                var user = new User()
                {
                    Username = data.Username,
                    Password = hashedPassword,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new ResponseSuccess()
                {
                    Message = "El usuario fue creado con exito.",
                    Status = 200
                };
            }
            catch (Exception ex) {

                throw ex;
            }
        }
    }
}
