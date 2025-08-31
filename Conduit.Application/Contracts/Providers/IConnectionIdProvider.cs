using Conduit.Domain.Common;

namespace Conduit.Application.Contracts.Providers;

public interface IConnectionIdProvider
{
    public Task<Result<IReadOnlyCollection<Guid>>> GetConnectionIds(CancellationToken ct = default);

    public Task<Dictionary<Guid, IReadOnlyCollection<Guid>>> GetConnectionIds(IReadOnlyCollection<Guid> userIds,
        CancellationToken ct = default);
}