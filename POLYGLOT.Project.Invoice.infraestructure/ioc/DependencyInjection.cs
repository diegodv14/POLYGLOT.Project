using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POLYGLOT.Project.Invoice.infraestructure.Handlers;
using POLYGLOT.Project.Invoice.application.Interfaces;
using POLYGLOT.Project.Invoice.application.Models;
using POLYGLOT.Project.Invoice.infraestructure.Repositories;
using RabbitMQ.Client;
using Microsoft.Extensions.Hosting;

namespace POLYGLOT.Project.Invoice.infraestructure.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbInvoiceContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddHostedService<UpdateInvoiceHandler>();
            services.AddScoped<IConnectionFactory>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new ConnectionFactory
                {
                    HostName = configuration["RabbitMQ:Host"],
                    UserName = configuration["RabbitMQ:User"],
                    Password = configuration["RabbitMQ:Pass"],
                    VirtualHost = configuration["RabbitMQ:VirtualHost"],
                };
            });
            services.AddScoped<IInvoices, InvoicesRepository>();
            return services;
        }
    }
}
