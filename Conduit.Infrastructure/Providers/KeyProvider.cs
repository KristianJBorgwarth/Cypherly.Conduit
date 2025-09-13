using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
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
        DateTimeOffset signedPreKeyTimestamp,
        CancellationToken cancellationToken)
    {
        var response = await _client.PostAsJsonAsync(
            "api/keys/",
            new
            {
                AccessKey = accessKey,
                IdentityKey = identityKey,
                RegistrationId = registrationId,
                SignedPrekeyId = signedPrekeyId,
                SignedPreKeyPublic = signedPreKeyPublic,
                SignedPreKeySignature = signedPreKeySignature,
                SignedPreKeyTimestamp = signedPreKeyTimestamp
            },
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("KeyClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync(cancellationToken);
        }
        
        return Result.Ok();
    }
}