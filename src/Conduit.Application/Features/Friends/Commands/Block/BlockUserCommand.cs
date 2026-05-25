using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Friends.Commands.Block
{
    public sealed record BlockUserCommand : ICommand
    {
        public required string BlockUserTag { get; init; }
    }
}
