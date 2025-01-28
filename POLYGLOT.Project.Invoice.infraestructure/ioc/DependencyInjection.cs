using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POLYGLOT.Project.Invoice.application.Models;

namespace POLYGLOT.Project.Invoice.infraestructure.ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbInvoiceContext>(opt => opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
