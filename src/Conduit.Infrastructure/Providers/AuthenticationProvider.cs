using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Authentication.Commands.Login;
using Conduit.Application.Features.Authentication.Commands.VerifyLogin;
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

    public async Task<Result<LoginDto>> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("login", new { Email = email, Password = password }, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<LoginDto>(ct);
        }

        return await response.GetValueFromEnvelopeAsync<LoginDto>(ct);
    }

    public async Task<Result> LogoutAsync(CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("logout", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync(ct);
        }

        return Result.Ok();
    }

    public async Task<Result<VerifyLoginDto>> VerifyLoginAsync(string loginVerificationCode, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("verify-login", new { LoginVerificationCode = loginVerificationCode }, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<VerifyLoginDto>(ct);
        }

        return await response.GetValueFromEnvelopeAsync<VerifyLoginDto>(ct);
    }
}
