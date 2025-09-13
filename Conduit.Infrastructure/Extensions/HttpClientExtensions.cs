using Conduit.Application.Contracts.Providers;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Providers;
using Conduit.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Conduit.Infrastructure.Extensions;

internal static class HttpClientExtensions
{
    internal static void ConfigureHttpClients(this IServiceCollection services)
    {
        services.AddHeaderPropagation(options =>
        {
            options.Headers.Add("Authorization");
        });
        
        var downStreamOptions =services.BuildServiceProvider().GetRequiredService<IOptions<DownstreamOptions>>().Value;
        
        services.AddClients(downStreamOptions);
        services.RegisterProviders();
    }

    private static void AddClients(this IServiceCollection services, DownstreamOptions options)
    {
        services.AddHttpClient(options, ClientNames.UserProfileClient);
        services.AddHttpClient(options, ClientNames.ConnectionIdClient);
        services.AddHttpClient(options, ClientNames.IdentityClient);
        services.AddHttpClient(options, ClientNames.KeyClient);
    }
    
    private static void RegisterProviders(this IServiceCollection services)
    {
        services.AddScoped<IUserProfileProvider, UserProfileProvider>();
        services.AddScoped<IUserProfileSettingsProvider, UserProfileSettingsProvider>();
        services.AddScoped<IFriendProvider, FriendProvider>();
        services.AddScoped<IConnectionIdProvider, ConnectionIdProvider>();
        services.AddScoped<IIdentityProvider, AuthenticationProvider>();
        services.AddScoped<IKeyProvider, KeyProvider>();
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

        httpClientBuilder.AddHeaderPropagation();
    }
}