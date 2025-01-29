using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POLYGLOT.Project.Security.application.Dto;
using POLYGLOT.Project.Security.application.Interfaces;
using POLYGLOT.Project.Security.application.Models;
using POLYGLOT.Project.Security.infraestructure.Repositories;

namespace POLYGLOT.Project.Security.infraestructure.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbSecurityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IGetToken, GetTokenRepository>();
            services.AddScoped<IUser,  UserRepository>();
            services.Configure<JwtSettings>(opt =>
            {
                opt.Issuer = configuration.GetSection("JWT:Issuer").Value;
                opt.Key = configuration.GetSection("JWT:Key").Value;
                opt.Enabled = bool.Parse(configuration.GetSection("JWT:Enabled").Value);
                opt.Expiration = int.Parse(configuration.GetSection("JWT:Expiration").Value);
                opt.Audience = configuration.GetSection("JWT:Audience").Value;

            });
            return services;
        }
    }
}
