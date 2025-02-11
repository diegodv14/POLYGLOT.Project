﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POLYGLOT.Project.Pay.application.Interfaces;
using POLYGLOT.Project.Pay.application.Models;
using POLYGLOT.Project.Pay.infraestructure.Repositories;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

namespace POLYGLOT.Project.Pay.infraestructure.Ioc
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["cn:db-pay-mqsl"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "La cadena de conexiòn no esta definida Micro.Pay");
            }
            services.AddDbContext<DbOperationContext>(opt => opt.UseMySQL(connectionString));

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

            services.AddScoped<IRabbitMQ, RabbitMQRepository>();
            services.AddScoped<IPayInvoice, PayInvoiceRepository>();
            services.AddHttpClient();
            return services;
        }
    }
}
