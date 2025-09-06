using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Friends.Queries.GetFriendRequests;

public sealed class GetFriendRequestsQueryHandler(
    IFriendProvider friendProvider,
    ILogger<GetFriendRequestsQueryHandler> logger) 
    : IQueryHandler<GetFriendRequestsQuery, IReadOnlyCollection<GetFriendRequestsDto>>
{
    public async Task<Result<IReadOnlyCollection<GetFriendRequestsDto>>> Handle(GetFriendRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var friendRequests = await friendProvider.GetFriendRequestsAsync(cancellationToken);
            return Result.Ok(friendRequests);
        }
        catch (Exception e)
        {
            logger.LogError("An exception occured while retrieving friend requests: {ExceptionMessage}", e.Message);
            return Result.Fail<IReadOnlyCollection<GetFriendRequestsDto>>(Error.Failure("An unexpected error occured"));
        }
    }
}