using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace POLYGLOT.Project.Pay.infraestructure.ioc
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbContext>(opt => opt.UseMySQL(configuration.GetConnectionString("DefaultConnection")));
            return services;
        }
    }
}
