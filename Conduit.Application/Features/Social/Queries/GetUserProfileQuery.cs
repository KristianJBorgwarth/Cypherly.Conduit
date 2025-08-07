using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Social.Queries;

public sealed record GetUserProfileQuery : IQuery<GetUserProfileDto>;