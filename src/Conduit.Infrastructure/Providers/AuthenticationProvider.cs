using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Authentication.Commands.Login;
using Conduit.Application.Features.Authentication.Commands.RefreshTokens;
using Conduit.Application.Features.Authentication.Commands.VerifyLogin;
using Conduit.Application.Features.Authentication.Commands.VerifyNonce;
using Conduit.Application.Features.Authentication.Queries;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class AuthenticationProvider(
    IHttpClientFactory clientFactory,
    ILogger<AuthenticationProvider> logger)
    : IIdentityProvider
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
        var response = await _client.PostAsync("logout", null, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync(ct);
        }

        return Result.Ok();
    }

    public async Task<Result<VerifyLoginDto>> VerifyLoginAsync(Guid userId, string loginVerificationCode, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("verify-login", new { UserId = userId, LoginVerificationCode = loginVerificationCode }, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<VerifyLoginDto>(ct);
        }

        return await response.GetValueFromEnvelopeAsync<VerifyLoginDto>(ct);
    }

    public async Task<Result<GetNonceDto>> GetNonceAsync(Guid userId, Guid deviceId, CancellationToken ct = default)
    {
        var response = await _client.GetAsync($"nonce?userId={userId}&deviceId={deviceId}", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<GetNonceDto>(ct);
        }

        return await response.GetValueFromEnvelopeAsync<GetNonceDto>(ct);

    }

    public async Task<Result<VerifyNonceDto>> VerifyNonceAsync(Guid userId, Guid nonceId, Guid deviceId, string nonce, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("verify-nonce", new { UserId = userId, NonceId = nonceId, DeviceId = deviceId, Nonce = nonce }, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<VerifyNonceDto>(ct);
        }

        return await response.GetValueFromEnvelopeAsync<VerifyNonceDto>(ct);
    }

    public async Task<Result<RefreshTokensDto>> RefreshTokensAsync(Guid userId, Guid deviceId, string refreshToken, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("refresh-token", new { UserId = userId, DeviceId = deviceId, RefreshToken = refreshToken }, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<RefreshTokensDto>(ct);
        }

        return await response.GetValueFromEnvelopeAsync<RefreshTokensDto>(ct);
    }

}
