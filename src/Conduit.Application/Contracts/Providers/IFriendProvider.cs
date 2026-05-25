using Conduit.Application.Features.Friends.Queries.GetFriendRequests;
using Conduit.Domain.Common;
using Conduit.Domain.Models;

namespace Conduit.Application.Contracts.Providers;

public interface IFriendProvider
{
    Task<Result> CreateFriendshipAsync(string friendTag, CancellationToken ct = default);
    Task<Result<IReadOnlyCollection<Friend>>> GetFriendsAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyCollection<GetFriendRequestsDto>>> GetFriendRequestsAsync(CancellationToken ct = default);
    Task<Result> DeleteFriendshipAsync(string friendTag, CancellationToken ct = default);
    Task<Result> BlockUserAsync(string userTag, CancellationToken ct = default);
}   
