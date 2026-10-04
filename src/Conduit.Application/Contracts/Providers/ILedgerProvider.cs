using Conduit.Domain.Common;

namespace Conduit.Application.Contracts.Providers;

public interface ILedgerProvider
{
    Task<Result> CreateLedgerAsync(
        Guid ledgerId,
        byte[] payload,
        byte[] writeKey,
        byte[] signature,
        CancellationToken ct = default);

    Task<Result> AppendEventAsync(
        Guid ledgerId,
        int expectedVersion,
        byte[] previousHash,
        byte[] payload,
        byte[] writeKeyPublic,
        byte[] signature,
        IReadOnlyCollection<byte[]> keysAdded,
        IReadOnlyCollection<byte[]> keysRemoved,
        CancellationToken ct = default);
}
