using POLYGLOT.Project.Invoice.infraestructure.Extensions;
using POLYGLOT.Project.Invoice.infraestructure.Ioc;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((host, config) => config.AddNacosConfiguration(config.Build().GetSection("nacos")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
