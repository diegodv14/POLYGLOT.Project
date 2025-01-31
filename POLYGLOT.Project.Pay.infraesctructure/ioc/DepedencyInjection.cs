using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POLYGLOT.Project.Pay.application.Interfaces;
using POLYGLOT.Project.Pay.application.Models;
using POLYGLOT.Project.Pay.infraestructure.Repositories;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace POLYGLOT.Project.Pay.infraestructure.Ioc
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "La cadena de conexiòn no esta definida Micro.Pay");
            }

            services.AddSingleton<IConnectionFactory>(sp =>
            {
                return new ConnectionFactory
                {
                    HostName = configuration["RabbitMQ:Host"],
                    UserName = configuration["RabbitMQ:User"],
                    Password = configuration["RabbitMQ:Pass"],
                    VirtualHost = configuration["RabbitMQ:VirtualHost"],
                };
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            services.AddScoped<IRabbitMQ, RabbitMQRepository>();
            services.AddScoped<IPayInvoice, PayInvoiceRepository>();
            services.AddDbContext<DbOperationContext>(opt => opt.UseMySQL(connectionString));
            services.AddHttpClient();
            return services;
        }
    }
}
