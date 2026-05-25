namespace Conduit.Application.Features.UserProfile.Commands.UpdateDisplayName;

public sealed record UpdateDisplayNameDto
{
    public required string DisplayName { get; init; }
}
