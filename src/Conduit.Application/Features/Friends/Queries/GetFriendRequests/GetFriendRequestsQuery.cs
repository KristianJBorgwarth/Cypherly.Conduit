using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Friends.Queries.GetFriendRequests;

public sealed record GetFriendRequestsQuery : IQuery<IReadOnlyCollection<GetFriendRequestsDto>>;