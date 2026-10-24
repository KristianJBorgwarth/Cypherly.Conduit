using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Friends.Queries.GetFriends;

public sealed class GetFriendsQueryHandler(IFriendProvider friendProvider)
    : IQueryHandler<GetFriendsQuery, IReadOnlyCollection<GetFriendsDto>>
{
    public async ValueTask<Result<IReadOnlyCollection<GetFriendsDto>>> Handle(GetFriendsQuery request, CancellationToken ct)
    {
        var friendsResult = await friendProvider.GetFriendsAsync(ct);
        if (!friendsResult.Success) return Result.Fail<IReadOnlyCollection<GetFriendsDto>>(friendsResult.Error);

        return friendsResult.RequiredValue.Select(f => new GetFriendsDto(f)).ToArray();
    }
}
