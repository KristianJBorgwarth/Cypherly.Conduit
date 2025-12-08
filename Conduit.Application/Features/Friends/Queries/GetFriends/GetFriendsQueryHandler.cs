using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Conduit.Domain.Models;

namespace Conduit.Application.Features.Friends.Queries.GetFriends;

public sealed class GetFriendsQueryHandler(
    IFriendProvider friendProvider,
    IConnectionIdProvider connectionIdProvider)
    : IQueryHandler<GetFriendsQuery, IReadOnlyCollection<GetFriendsDto>>
{
    public async Task<Result<IReadOnlyCollection<GetFriendsDto>>> Handle(GetFriendsQuery request, CancellationToken ct)
    {
        var friendsResult = await friendProvider.GetFriendsAsync(ct);
        if (!friendsResult.Success) return Result.Fail<IReadOnlyCollection<GetFriendsDto>>(friendsResult.Error);

        var friendIds = friendsResult.RequiredValue.Select(f => f.Id).ToList();
        var conIdsResult = await connectionIdProvider.GetConnectionIds(friendIds, ct);
        if (!conIdsResult.Success) return Result.Fail<IReadOnlyCollection<GetFriendsDto>>(conIdsResult.Error);

        var dtos = MapToDtos(friendsResult.RequiredValue, conIdsResult.RequiredValue);

        return dtos;
    }

    private static GetFriendsDto[] MapToDtos(IReadOnlyCollection<Friend> friends, Dictionary<Guid, IReadOnlyCollection<Guid>> connectionIds) =>
        [.. friends.Select(f => new GetFriendsDto(f, connectionIds[f.Id]))];
}
