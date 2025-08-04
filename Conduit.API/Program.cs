using System.Reflection;
using Conduit.API.Extensions;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Appsettings

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

#endregion

#region Logger

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddObservability(configuration);

Serilog.Debugging.SelfLog.Enable(Console.Error);

#endregion

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

#region Scalar

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Conduit.API")
        .WithTheme(ScalarTheme.Purple)
        .WithDarkModeToggle(false)
        .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
});

#endregion

app.MapPrometheusScrapingEndpoint();
app.UseSerilogRequestLogging();

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