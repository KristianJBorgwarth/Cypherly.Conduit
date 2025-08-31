
using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Friends.Commands.Create;

public sealed record CreateFriendshipCommand : ICommand
{
    public required string FriendTag { get; init; }
}