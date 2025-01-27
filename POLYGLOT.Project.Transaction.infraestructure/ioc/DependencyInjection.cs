using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using POLYGLOT.Project.Transaction.application;


namespace POLYGLOT.Project.Transaction.infraestructure.ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MongoSettings>(opt =>
            {
                opt.ConnectionString = configuration.GetSection("MongoSettings:ConnectionString").Value;
                opt.DatabaseName = configuration.GetSection("MongoSettings:DatabaseName").Value;
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

            return services;
        }
    }
}
