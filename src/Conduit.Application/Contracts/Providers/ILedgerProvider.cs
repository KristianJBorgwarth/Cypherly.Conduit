using Conduit.Application.Features.Ledger.Dtos;
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

    Task<Result> ArchiveLedgerAsync(
        Guid ledgerId,
        int expectedVersion,
        byte[] previousHash,
        byte[] payload,
        byte[] writeKeyPublic,
        byte[] signature,
        CancellationToken ct = default);

    Task<Result<LedgerEventsDto>> GetEventsAsync(
        Guid ledgerId,
        int fromVersion,
        int? limit,
        CancellationToken ct = default);
}
