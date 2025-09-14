using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Conduit.Domain.Models;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class KeyProvider(
    IHttpClientFactory clientFactory,
    ILogger<KeyProvider> logger)
    : IKeyProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.KeyClient);

    public async Task<Result> CreateKeyBundleAsync(
        Guid accessKey,
        byte[] identityKey,
        ushort registrationId,
        int signedPrekeyId,
        byte[] signedPreKeyPublic,
        byte[] signedPreKeySignature,
        IReadOnlyCollection<PreKey> preKeys,
        DateTimeOffset signedPreKeyTimestamp,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            "keys",
            new
            {
                AccessKey = accessKey,
                IdentityKey = identityKey,
                RegistrationId = registrationId,
                SignedPrekeyId = signedPrekeyId,
                SignedPreKeyPublic = signedPreKeyPublic,
                PreKeys = preKeys,
                SignedPreKeySignature = signedPreKeySignature,
                SignedPreKeyTimestamp = signedPreKeyTimestamp
            },
            ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("KeyClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync(ct, fromDetails: true);
        }

        return Result.Ok();
    }

    public async Task<Result> UploadOneTimePreKeysAsync(IReadOnlyCollection<PreKey> preKeys,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("otps", new { PreKeys = preKeys }, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("KeyClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync(ct, fromDetails: true);
        }

        return Result.Ok();
    }
}