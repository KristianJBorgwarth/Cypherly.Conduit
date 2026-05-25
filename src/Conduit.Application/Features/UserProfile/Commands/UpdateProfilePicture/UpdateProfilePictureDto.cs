namespace Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;

public sealed record UpdateProfilePictureDto
{
    public required string? ProfilePictureUrl { get; init; }
}