using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Friends.Commands.Create;

public sealed class CreateFriendshipCommandHandler(
    IFriendProvider friendProvider,
    ILogger<CreateFriendshipCommandHandler> logger)
    : ICommandHandler<CreateFriendshipCommand>
{
    public async Task<Result> Handle(CreateFriendshipCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await friendProvider.CreateFriendshipAsync(request.FriendTag, cancellationToken);
            return Result.Ok();
        }
        catch (Exception ex)
        {
            logger.LogError("Error creating friendship with tag {FriendTag}: {ErrorMessage}", request.FriendTag, ex.Message);
            return Result.Fail(Error.Failure("Internal.Server.Error", "An error occurred while creating the friendship."));
        }        
    }
}