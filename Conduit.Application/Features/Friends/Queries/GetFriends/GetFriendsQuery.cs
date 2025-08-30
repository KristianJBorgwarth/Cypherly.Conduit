using Conduit.Application.Abstractions;
namespace Conduit.Application.Features.Friends.Queries.GetFriends;
    
public sealed record GetFriendsQuery : IQuery<IReadOnlyCollection<GetFriendsDto>>;