namespace Conduit.Application.Contracts.Providers;

public interface IConnectionIdProvider
{
    public List<Guid> GetConnectionIds();
}