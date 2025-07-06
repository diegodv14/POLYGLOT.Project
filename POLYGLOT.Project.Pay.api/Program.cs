using Microsoft.EntityFrameworkCore;
using POLYGLOT.Project.Pay.application.Models;
using POLYGLOT.Project.Pay.infraestructure.Extensions;
using POLYGLOT.Project.Pay.infraestructure.Ioc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfraestructure(builder.Configuration);

builder.Services.AddDbContext<DbOperationContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

builder.Services.AddSwaggerGen();

builder.Host.UseNacosConfig(section: "nacos");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureExceptionHandler();

app.MapControllers();

app.Run();
