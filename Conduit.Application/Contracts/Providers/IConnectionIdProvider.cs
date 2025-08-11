namespace Conduit.Application.Contracts.Providers;

public interface IConnectionIdProvider
{
    public Task<List<Guid>> GetConnectionIds(CancellationToken cancellationToken = default);
}