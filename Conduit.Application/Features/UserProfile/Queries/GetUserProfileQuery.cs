using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.UserProfile.Queries;

public sealed record GetUserProfileQuery : IQuery<GetUserProfileDto>
{
    public required Guid ExclusiveConnectionId { get; init; }
}
