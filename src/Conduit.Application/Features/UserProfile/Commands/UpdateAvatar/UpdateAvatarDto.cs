namespace Conduit.Application.Features.UserProfile.Commands.UpdateAvatar;

public sealed record UpdateAvatarDto
{
    public required Guid FileKey { get; init; }
}
