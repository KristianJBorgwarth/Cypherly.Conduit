using Conduit.Application.Features.Friends.Dtos;
// ReSharper disable ConvertToPrimaryConstructor

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;

public sealed class GetUserProfileByTagDto
{
    public string Username { get; private init; }
    public string UserTag { get; private init; }
    public string? DisplayName { get; private init; }
    public Guid? AvatarKey { get; private init; }
    public FriendshipStatusDto FriendshipStatus { get; private init; }

    public GetUserProfileByTagDto(
        string username, 
        string userTag, 
        string? displayName, 
        Guid? avatarKey, 
        FriendshipStatusDto friendshipStatus)
    {
        Username = username;
        UserTag = userTag;
        DisplayName = displayName;
        AvatarKey = avatarKey;
        FriendshipStatus = friendshipStatus;
    }
}
