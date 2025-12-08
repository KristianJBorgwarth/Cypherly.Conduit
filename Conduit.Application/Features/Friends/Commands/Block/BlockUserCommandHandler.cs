using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Friends.Commands.Block;

public sealed class BlockUserCommandHandler(IFriendProvider friendProvider) : ICommandHandler<BlockUserCommand>
{
    public async Task<Result> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        return await friendProvider.BlockUserAsync(request.BlockUserTag, cancellationToken);
    }
}
