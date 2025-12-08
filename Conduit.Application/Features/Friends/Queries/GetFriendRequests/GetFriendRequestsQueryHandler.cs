using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Friends.Queries.GetFriendRequests;

public sealed class GetFriendRequestsQueryHandler(IFriendProvider friendProvider) : IQueryHandler<GetFriendRequestsQuery, IReadOnlyCollection<GetFriendRequestsDto>>
{
    public async Task<Result<IReadOnlyCollection<GetFriendRequestsDto>>> Handle(GetFriendRequestsQuery request, CancellationToken cancellationToken)
    {
        var friendRequests = await friendProvider.GetFriendRequestsAsync(cancellationToken);
        return Result.Ok(friendRequests);
    }
}
