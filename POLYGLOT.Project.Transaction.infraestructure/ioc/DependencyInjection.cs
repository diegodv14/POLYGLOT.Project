﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace POLYGLOT.Project.Transaction.infraestructure.ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
