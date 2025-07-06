using Microsoft.EntityFrameworkCore;
using POLYGLOT.Project.Security.infraestructure.Ioc;
using POLYGLOT.Project.Security.infraestructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNacosConfig(section: "nacos");
builder.Services.AddControllers();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<POLYGLOT.Project.Security.application.Models.DbSecurityContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.ConfigureExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
