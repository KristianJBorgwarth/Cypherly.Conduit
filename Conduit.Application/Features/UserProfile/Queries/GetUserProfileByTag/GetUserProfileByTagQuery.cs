using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;

public sealed record GetUserProfileByTagQuery : IQuery<GetUserProfileByTagDto>
{
    public required string UserTag { get; init; }
}
