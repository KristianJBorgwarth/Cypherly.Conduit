using Conduit.Domain.Models;

namespace Conduit.Application.Contracts.Providers;

public interface IFriendProvider
{
    Task<IReadOnlyCollection<Friend>> GetFriendsAsync(CancellationToken ct = default);
    Task CreateFriendshipAsync(string friendTag, CancellationToken ct = default);
}