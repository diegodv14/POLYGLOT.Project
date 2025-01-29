using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POLYGLOT.Project.Pay.application.Interfaces;
using POLYGLOT.Project.Pay.application.Models;
using POLYGLOT.Project.Pay.infraestructure.Repositories;
using RabbitMQ.Client;

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

            services.AddScoped<IRabbitMQ, RabbitMQRepository>();
            services.AddDbContext<DbOperationContext>(opt => opt.UseMySQL(connectionString));
            services.AddHttpClient();
            return services;
        }
    }
}
