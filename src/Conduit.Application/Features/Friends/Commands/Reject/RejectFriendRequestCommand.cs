using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Friends.Commands.Reject;

public sealed record RejectFriendRequestCommand : ICommand
{
    public required string FriendTag { get; init; }
}
