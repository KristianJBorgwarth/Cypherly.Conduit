using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Friends.Commands.Delete;

public sealed class DeleteFriendshipCommandHandler(
    IFriendProvider provider,
    ILogger<DeleteFriendshipCommandHandler> logger)
    : ICommandHandler<DeleteFriendshipCommand>
{
    public async Task<Result> Handle(DeleteFriendshipCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await provider.DeleteFriendshipAsync(request.FriendTag, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error deleting friendship with {FriendTag}", request.FriendTag); 
            return Result.Fail(Error.Failure("An internal server error occurred."));
        }
    }
}