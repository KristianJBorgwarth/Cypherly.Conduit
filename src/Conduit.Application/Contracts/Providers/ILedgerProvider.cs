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
}
