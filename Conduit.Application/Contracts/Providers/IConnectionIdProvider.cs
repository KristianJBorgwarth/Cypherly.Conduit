namespace Conduit.Application.Contracts.Providers;

public interface IConnectionIdProvider
{
    public Task<IReadOnlyCollection<Guid>> GetConnectionIds(CancellationToken cancellationToken = default);
}