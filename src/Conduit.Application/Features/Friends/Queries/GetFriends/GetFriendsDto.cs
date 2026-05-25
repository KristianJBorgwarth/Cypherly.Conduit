using Conduit.Domain.Models;
// ReSharper disable ConvertToPrimaryConstructor

namespace Conduit.Application.Features.Friends.Queries.GetFriends;

public sealed class GetFriendsDto
{
    public string Username { get; private init; }
    public string UserTag { get; private init; }
    public string? DisplayName { get; private init; }
    public string? ProfilePictureUrl { get; private init; }
    public IReadOnlyCollection<Guid> ConnectionIds { get; private init; }

    public GetFriendsDto(Friend friend, IReadOnlyCollection<Guid> connectionIds)
    {
        Username = friend.Username;
        UserTag = friend.UserTag;
        DisplayName = friend.DisplayName;
        ProfilePictureUrl = friend.ProfilePictureUrl;
        ConnectionIds = connectionIds;
    }
}