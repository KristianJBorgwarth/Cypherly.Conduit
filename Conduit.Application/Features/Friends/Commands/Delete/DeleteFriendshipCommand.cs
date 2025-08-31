using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Friends.Commands.Delete;

public sealed record DeleteFriendshipCommand : ICommand
{
    public required string FriendTag { get; init; }
}