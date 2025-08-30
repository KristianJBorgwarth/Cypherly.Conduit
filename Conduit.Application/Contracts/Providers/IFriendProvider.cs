using Conduit.Domain.Models;

namespace Conduit.Application.Contracts.Providers;

public interface IFriendProvider
{
    Task<IReadOnlyCollection<Friend>> GetFriends(CancellationToken ct = default);
}