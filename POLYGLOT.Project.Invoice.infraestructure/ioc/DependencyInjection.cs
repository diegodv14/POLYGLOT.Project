using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                    HostName = configuration.GetConnectionString("RabbitMQ:Host"),
                    UserName = configuration.GetConnectionString("RabbitMQ:User"),
                    Password = configuration.GetConnectionString("RabbitMQ:Pass"),
                    VirtualHost = configuration.GetConnectionString("RabbitMQ:VirtualHost"),
                };
            });
            services.AddHostedService<UpdateInvoiceHandler>();
            services.AddScoped<IInvoices, InvoicesRepository>();
            return services;
        }
    }
}
