using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POLYGLOT.Project.Invoice.application.Interfaces;
using POLYGLOT.Project.Invoice.application.Models;
using POLYGLOT.Project.Invoice.infraestructure.Consumers;
using POLYGLOT.Project.Invoice.infraestructure.Repositories;
using RabbitMQ.Client;

namespace POLYGLOT.Project.Invoice.infraestructure.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbInvoiceContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
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

            services.AddHostedService<UpdateInvoiceHandler>();
            services.AddScoped<IInvoices, InvoicesRepository>();
            return services;
        }
    }
}
