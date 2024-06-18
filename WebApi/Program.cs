

using WebApi.Helpers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using WebApi.Middleware;
using WebApi.Extensions;
_.__();

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    builder.WebHost.UseUrls("http://localhost:3000");
    builder.WebHost.ConfigureLogging((context, logging) =>
    {
        var config = context.Configuration.GetSection("Logging");
        logging.AddConfiguration(config);
        logging.AddConsole();
        logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
        logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);
        logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
    });

    var services = builder.Services;
    var configuration = builder.Configuration;
    services.AddControllers();

    services.AddSwaggerGen();

    services.AddApplicationServices();
    services.ConfigureSettings(configuration);

    services.AddSqlite<DataContext>("DataSource=webApi.db");

    services.AddDataProtection().UseCryptographicAlgorithms(
        new AuthenticatedEncryptorConfiguration
        {
            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
            ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
        });
}

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.EnsureCreated();
}

// configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseMiddleware<ValidationExceptionMiddleware>();
app.UseMiddleware<ErrorHandler>();

{
    app.MapControllers();
}

app.Run();

public partial class Program { }
