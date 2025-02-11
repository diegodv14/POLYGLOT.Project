﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using POLYGLOT.Project.Transaction.application;
using POLYGLOT.Project.Transaction.application.Interfaces;
using POLYGLOT.Project.Transaction.infraestructure.Consumer;
using POLYGLOT.Project.Transaction.infraestructure.Repositories;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;


namespace POLYGLOT.Project.Transaction.infraestructure.ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"])),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

            services.Configure<MongoSettings>(opt =>
            {
                opt.ConnectionString = configuration["cn:db-mongo-transaccion"];
                opt.DatabaseName = configuration["cn:databaseName"];
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

            services.AddHostedService<AddTransaccionHandlerConsumer>();

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor inserte el JWT con Bearer en el campo",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });


                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

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

            return services;
        }
    }
}
