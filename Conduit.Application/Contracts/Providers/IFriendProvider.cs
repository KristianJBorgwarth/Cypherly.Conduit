using Conduit.Application.Features.Friends.Queries.GetFriendRequests;
using Conduit.Domain.Models;

namespace Conduit.Application.Contracts.Providers;

public interface IFriendProvider
{
    Task CreateFriendshipAsync(string friendTag, CancellationToken ct = default);
    Task<IReadOnlyCollection<Friend>> GetFriendsAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<GetFriendRequestsDto>> GetFriendRequestsAsync(CancellationToken ct = default);
    Task DeleteFriendshipAsync(string friendTag, CancellationToken ct = default);
}   