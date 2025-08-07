using Conduit.Domain.Models;
// ReSharper disable ConvertToPrimaryConstructor

namespace Conduit.Application.Features.Social.Queries;

public sealed class GetUserProfileDto
{
    public Guid Id { get; private init; }
    public string Username { get; private init; }
    public string UserTag { get; private init; }
    public string? ProfilePictureUrl { get; private init; }
    public string? DisplayName { get; private init; }
    public IReadOnlyCollection<Guid>  ConnectionIds { get; private init; }
 
    public GetUserProfileDto(UserProfile profile, IReadOnlyCollection<Guid> connectionIds)
    {
        Id = profile.Id;
        Username = profile.Username;
        UserTag = profile.UserTag;
        ProfilePictureUrl = profile.ProfilePictureUrl;
        DisplayName = profile.DisplayName;
        ConnectionIds = connectionIds;
    }
}