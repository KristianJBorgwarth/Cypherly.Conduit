using Conduit.Application.Contracts.Providers;
using Conduit.Infrastructure.Clients;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Conduit.Infrastructure.Extensions;

internal static class HttpClientExtensions
{
    internal static void ConfigureHttpClients(this IServiceCollection services)
    {
        var downStreamOptions =services.BuildServiceProvider().GetRequiredService<IOptions<DownstreamOptions>>().Value;
        
        // Clients
        services.AddHttpClient(downStreamOptions, ClientNames.UserProfileClient);
        services.AddHttpClient(downStreamOptions, ClientNames.ConnectionIdClient);

        // Services
        services.AddScoped<IUserProfileProvider, UserProfileClient>();
        services.AddScoped<IConnectionIdProvider, ConnectionIdClient>();
    }

    private static void AddHttpClient(this IServiceCollection services, DownstreamOptions options, string clientName)
    {
        if(string.IsNullOrWhiteSpace(clientName))
            throw new ArgumentNullException(nameof(clientName));

        var downstream = options.GetDownStream(clientName);
        
        if(!Uri.TryCreate(downstream.Url, UriKind.Absolute, out var uri))
            throw new ArgumentException($"Downstream url '{downstream.Url}' is not a valid URI");

        var httpClientBuilder = services.AddHttpClient(clientName, client =>
        {
            client.BaseAddress = uri;
        });

        if (!downstream.UsesAuthentication) return;
        
        httpClientBuilder.AddHeaderPropagation(headerOptions => headerOptions.Headers.Add("Authorization"));
    }
}