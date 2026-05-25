using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfile;

public sealed record GetUserProfileQuery : IQuery<GetUserProfileDto>
{
    public required Guid ExclusiveConnectionId { get; init; }
}
