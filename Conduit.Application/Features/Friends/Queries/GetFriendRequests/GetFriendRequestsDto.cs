// ReSharper disable ConvertToPrimaryConstructor
namespace Conduit.Application.Features.Friends.Queries.GetFriendRequests;

public sealed class GetFriendRequestsDto
{
    public string Username { get; private init; }
    public string UserTag { get; private init; }
    public string? DisplayName { get; private init; }
    public string? ProfilePictureUrl { get; private init; }
    
    public GetFriendRequestsDto(string username, string userTag, string? displayName, string? profilePictureUrl)
    {
        Username = username;
        UserTag = userTag;
        DisplayName = displayName;
        ProfilePictureUrl = profilePictureUrl;
    }
}
