using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Friends.Commands.Block
{
    public sealed class BlockUserCommandHandler(
        IFriendProvider friendProvider,
        ILogger<BlockUserCommandHandler> logger)
        : ICommandHandler<BlockUserCommand>
    {
        public async Task<Result> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await friendProvider.BlockUserAsync(request.BlockUserTag, cancellationToken);
            }
            catch (Exception)
            {
                logger.LogError("Error blocking user with tag {BlockUserTag}", request.BlockUserTag);
                return Result.Fail(Error.Failure("An error occurred while blocking the user."));
            }
        }
    }
}
