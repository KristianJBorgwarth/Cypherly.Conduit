using Conduit.Application.Contracts.Providers;

namespace Conduit.Infrastructure.Clients;

internal sealed class ConnectionIdClient : IConnectionIdProvider
{
    public List<Guid> GetConnectionIds()
    {
        throw new NotImplementedException();
    }
}