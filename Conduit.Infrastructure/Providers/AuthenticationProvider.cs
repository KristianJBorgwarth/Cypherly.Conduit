using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Authentication.Commands.Login;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class AuthenticationProvider(
    IHttpClientFactory clientFactory,
    ILogger<AuthenticationProvider> logger) : IIdentityProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.IdentityClient);
    
    public async Task<Result<LoginDto>> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var response = await _client.PostAsJsonAsync("login", new { Email = email, Password = password }, cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<LoginDto>(cancellationToken);
        }
        
        return await response.GetValueFromEnvelopeAsync<LoginDto>(cancellationToken);
    }
}