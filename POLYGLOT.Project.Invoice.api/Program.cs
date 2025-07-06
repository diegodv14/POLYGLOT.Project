using Microsoft.EntityFrameworkCore;
using POLYGLOT.Project.Invoice.application.Models;
using POLYGLOT.Project.Invoice.infraestructure.Extensions;
using POLYGLOT.Project.Invoice.infraestructure.Ioc;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNacosConfig(section: "nacos");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DbInvoiceContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.ConfigureExceptionHandler();

app.Run();
