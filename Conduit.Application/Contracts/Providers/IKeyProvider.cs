using Conduit.Domain.Common;

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
        DateTimeOffset signedPreKeyTimestamp,
        CancellationToken cancellationToken);
}