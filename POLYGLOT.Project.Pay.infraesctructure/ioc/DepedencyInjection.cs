using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace POLYGLOT.Project.Pay.infraestructure.ioc
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

            services.AddDbContext<DbContext>(opt => opt.UseMySQL(connectionString));
            return services;
        }
    }
}
