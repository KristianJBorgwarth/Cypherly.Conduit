using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Account.Commands.Register;
using Conduit.Application.Features.Device.Commands.Create;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class IdentityUserProvider(
    IHttpClientFactory clientFactory,
    ILogger<IdentityUserProvider> logger)
    : IIdentityUserProvider
{
    private static readonly Error Unreachable = Error.Failure("An unexpected error occurred.");

    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.IdentityUserClient);

    public async Task<Result<RegisterDto>> RegisterAsync(string username, string email, string password, CancellationToken ct = default)
    {
        var response = await SendAsync(() => _client.PostAsJsonAsync("", new { Username = username, Email = email, Password = password }, ct), ct);

        if (response is null)
            return Result.Fail<RegisterDto>(Unreachable);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityUserClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToPassthroughFailureResultAsync<RegisterDto>(ct);
        }

        return await response.GetValueAsync<RegisterDto>(ct);
    }

    public async Task<Result> VerifyAsync(Guid userId, string verificationCode, CancellationToken ct = default)
    {
        var response = await SendAsync(() => _client.PutAsJsonAsync("verify", new { UserId = userId, VerificationCode = verificationCode }, ct), ct);

        if (response is null)
            return Result.Fail(Unreachable);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityUserClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToPassthroughFailureResultAsync(ct);
        }

        return Result.Ok();
    }

    public async Task<Result> ResendVerificationCodeAsync(Guid userId, int codeType, Guid? nonceId, string? nonce, CancellationToken ct = default)
    {
        var response = await SendAsync(() => _client.PutAsJsonAsync("resend-verification", new { UserId = userId, CodeType = codeType, NonceId = nonceId, Nonce = nonce }, ct), ct);

        if (response is null)
            return Result.Fail(Unreachable);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityUserClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToPassthroughFailureResultAsync(ct);
        }

        return Result.Ok();
    }

    public async Task<Result<CreateDeviceDto>> CreateDeviceAsync(
        Guid userId,
        Guid loginNonceId,
        string loginNonce,
        string deviceAppVersion,
        int deviceType,
        int devicePlatform,
        string base64DevicePublicKey,
        CancellationToken ct = default)
    {
        var response = await SendAsync(() => _client.PostAsJsonAsync("device", new
        {
            UserId = userId,
            LoginNonceId = loginNonceId,
            LoginNonce = loginNonce,
            DeviceAppVersion = deviceAppVersion,
            DeviceType = deviceType,
            DevicePlatform = devicePlatform,
            Base64DevicePublicKey = base64DevicePublicKey,
        }, ct), ct);

        if (response is null)
            return Result.Fail<CreateDeviceDto>(Unreachable);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityUserClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToPassthroughFailureResultAsync<CreateDeviceDto>(ct);
        }

        return await response.GetValueAsync<CreateDeviceDto>(ct);
    }

    public async Task<Result> DeleteDeviceAsync(Guid targetDeviceId, CancellationToken ct = default)
    {
        var response = await SendAsync(() => _client.DeleteAsync($"device/{targetDeviceId}", ct), ct);

        if (response is null)
            return Result.Fail(Unreachable);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityUserClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToPassthroughFailureResultAsync(ct);
        }

        return Result.Ok();
    }

    private async Task<HttpResponseMessage?> SendAsync(Func<Task<HttpResponseMessage>> send, CancellationToken ct)
    {
        try
        {
            return await send();
        }
        catch (Exception ex) when (ex is HttpRequestException || (ex is TaskCanceledException && !ct.IsCancellationRequested))
        {
            logger.LogError(ex, "IdentityUserClient could not reach Identity");
            return null;
        }
    }
}
