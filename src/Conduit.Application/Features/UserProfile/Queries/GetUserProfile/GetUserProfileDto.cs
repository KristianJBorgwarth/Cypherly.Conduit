

// ReSharper disable ConvertToPrimaryConstructor
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfile;

public sealed class GetUserProfileDto
{
    public Guid Id { get; private init; }
    public string Username { get; private init; }
    public string UserTag { get; private init; }
    public Guid? AvatarKey { get; private init; }
    public string? DisplayName { get; private init; }
    public IReadOnlyCollection<Guid>  ConnectionIds { get; private init; }
 
    public GetUserProfileDto(Domain.Models.UserProfile profile, IReadOnlyCollection<Guid> connectionIds)
    {
        Id = profile.Id;
        Username = profile.Username;
        UserTag = profile.UserTag;
        AvatarKey = profile.AvatarKey;
        DisplayName = profile.DisplayName;
        ConnectionIds = connectionIds;
    }
}
