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

    public GetUserProfileByTagDto(Domain.Models.UserProfile userProfile, FriendshipStatus status, FriendshipDirection direction)
    {
        Username = userProfile.Username;
        UserTag = userProfile.UserTag;
        DisplayName = userProfile.DisplayName;
        ProfilePictureUrl = userProfile.ProfilePictureUrl;
        FriendshipStatus = new FriendshipStatusDto(status, direction);
    }
}
