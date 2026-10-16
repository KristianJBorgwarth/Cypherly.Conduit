using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Friends.Commands.Accept;

public sealed record AcceptFriendshipCommand : ICommand<AcceptFriendshipDto>
{
    public required string FriendTag { get; init; }
}
