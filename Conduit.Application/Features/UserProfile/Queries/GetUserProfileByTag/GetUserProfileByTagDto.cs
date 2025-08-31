using Conduit.Application.Dtos;
using Conduit.Domain.Enums;
// ReSharper disable ConvertToPrimaryConstructor

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;

public sealed class GetUserProfileByTagDto
{
    public string Username { get; private init; }
    public string UserTag { get; private init; }
    public string? DisplayName { get; private init; }
    public string? ProfilePictureUrl { get; private init; }
    public FriendshipStatusDto FriendshipStatus { get; private init; }

    public GetUserProfileByTagDto(string username, string userTag, string? displayName, string? profilePictureUrl, FriendshipStatusDto friendshipStatus)
    {
        Username = username;
        UserTag = userTag;
        DisplayName = displayName;
        ProfilePictureUrl = profilePictureUrl;
        FriendshipStatus = friendshipStatus;
    }
}
