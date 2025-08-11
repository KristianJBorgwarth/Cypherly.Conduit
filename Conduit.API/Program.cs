using System.Reflection;
using Conduit.API.Extensions;
using Conduit.Infrastructure.Extensions;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var env = builder.Environment;

var configuration = builder.Configuration;
configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables();

if (env.IsDevelopment())
{
    configuration.AddJsonFile($"appsettings.{Environments.Development}.json", true, true);
    configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
}

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddObservability(configuration);

Serilog.Debugging.SelfLog.Enable(Console.Error);

builder.Services.AddInfrastructure(configuration);


builder.Services.AddSecurity(configuration);

builder.Services.AddEndpoints();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Conduit.API")
        .WithTheme(ScalarTheme.Purple)
        .WithDarkModeToggle(false)
        .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
});


app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();


app.RegisterMinimalEndpoints();
app.MapPrometheusScrapingEndpoint();

try
{
    Log.Information("Cypherly.Conduit.API starting up");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

// Required for integration tests
public partial class Program { }