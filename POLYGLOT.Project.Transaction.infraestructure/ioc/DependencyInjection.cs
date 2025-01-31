﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using POLYGLOT.Project.Transaction.application;
using POLYGLOT.Project.Transaction.application.Interfaces;
using POLYGLOT.Project.Transaction.infraestructure.Repositories;


namespace POLYGLOT.Project.Transaction.infraestructure.ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MongoSettings>(opt =>
            {
                opt.ConnectionString = configuration["MongoSettings:ConnectionString"];
                opt.DatabaseName = configuration["MongoSettings:DatabaseName"];
            });


            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var mongoSettings = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;
                return new MongoClient(mongoSettings.ConnectionString);
            });

            services.AddScoped<IMongoDatabase>(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var mongoSettings = serviceProvider.GetRequiredService<IOptions<MongoSettings>>().Value;
                return client.GetDatabase(mongoSettings.DatabaseName);
            });
            services.AddScoped<ITransaccion, TransaccionRepository>();

            return services;
        }
    }
}
