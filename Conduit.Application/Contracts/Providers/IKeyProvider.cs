using Conduit.Domain.Common;
using Conduit.Domain.Models;

namespace Conduit.Application.Contracts.Providers;

public interface IKeyProvider
{
    Task<Result> CreateKeyBundleAsync(
        Guid accessKey,
        byte[] identityKey,
        ushort registrationId,
        int signedPrekeyId,
        byte[] signedPreKeyPublic,
        byte[] signedPreKeySignature,
        IReadOnlyCollection<PreKey> preKeys,
        DateTimeOffset signedPreKeyTimestamp,
        CancellationToken ct = default);
    
    Task<Result> UploadOneTimePreKeysAsync(IReadOnlyCollection<PreKey> preKeys, CancellationToken ct = default);
}