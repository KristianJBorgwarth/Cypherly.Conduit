using System.Reflection;
using Conduit.API.Extensions;
using Conduit.Application.Extensions;
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
    configuration.AddJsonFile("appsettings.Local.json", true, true);
    configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
}

if (env.IsProduction())
{
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();

    builder.Host.UseSerilog();

    builder.Services.AddObservability(configuration);

    Serilog.Debugging.SelfLog.Enable(Console.Error);
}

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddSecurity(configuration);
builder.Services.AddEndpoints();
builder.Services.AddOpenApi();
builder.Services.AddAntiforgery();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowElectron", policy =>
    {
        var allowedOrigins = configuration.GetSection("AllowedOrigins").Get<string[]>();
        policy.WithOrigins(allowedOrigins!)
            .AllowAnyMethod()
            .AllowCredentials()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Conduit.API")
        .WithTheme(ScalarTheme.Purple)
        .WithDarkModeToggle(false)
        .WithDefaultHttpClient(ScalarTarget.JavaScript, ScalarClient.Axios);
});


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseCors("AllowElectron");

app.UseAuthorization();

app.UseHeaderPropagation();

app.UseAntiforgery();

app.RegisterMinimalEndpoints();

if (env.IsProduction())
{
    app.UseSerilogRequestLogging();
    app.MapPrometheusScrapingEndpoint();
}

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
public partial class Program;
