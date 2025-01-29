﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POLYGLOT.Project.Pay.application.Models;

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

            services.AddDbContext<DbOperationContext>(opt => opt.UseMySQL(connectionString));
            services.AddHttpClient();
            return services;
        }
    }
}
