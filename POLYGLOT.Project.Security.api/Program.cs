using POLYGLOT.Project.Security.infraestructure.Ioc;
using POLYGLOT.Project.Security.infraestructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Host.ConfigureAppConfiguration((host, config) => config.AddNacosConfiguration(config.Build().GetSection("nacos")));

builder.Services.AddControllers();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
