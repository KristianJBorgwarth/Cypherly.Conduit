using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Conduit.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Friends.Queries.GetFriends;

public sealed class GetFriendsQueryHandler(
    IFriendProvider friendProvider,
    IConnectionIdProvider connectionIdProvider,
    ILogger<GetFriendsQueryHandler> logger)
    : IQueryHandler<GetFriendsQuery, IReadOnlyCollection<GetFriendsDto>>
{
    public async Task<Result<IReadOnlyCollection<GetFriendsDto>>> Handle(GetFriendsQuery request, CancellationToken ct)
    {
        try
        {
            var friends = await friendProvider.GetFriends(ct);
            if (friends.Count is 0) return Array.Empty<GetFriendsDto>();
            
            var friendIds = friends.Select(f => f.Id).ToList();
            var connectionIds = await connectionIdProvider.GetConnectionIds(friendIds, ct);
            
            var dtos = MapToDtos(friends, connectionIds);

            return dtos;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occured while retrieving friends for Tenant");
            return Result.Fail<IReadOnlyCollection<GetFriendsDto>>(Error.Failure( "Internal.Server.Error","An unexpected error occured"));
        }
    }
    
    private static GetFriendsDto[] MapToDtos(IReadOnlyCollection<Friend> friends, Dictionary<Guid, IReadOnlyCollection<Guid>> connectionIds) => 
        friends.Select(f => new GetFriendsDto(f, connectionIds[f.Id])).ToArray();
}