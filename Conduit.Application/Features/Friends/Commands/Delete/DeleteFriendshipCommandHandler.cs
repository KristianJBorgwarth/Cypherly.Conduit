using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Friends.Commands.Delete;

public sealed class DeleteFriendshipCommandHandler(IFriendProvider provider) : ICommandHandler<DeleteFriendshipCommand>
{
    public async Task<Result> Handle(DeleteFriendshipCommand request, CancellationToken cancellationToken)
    {
        return await provider.DeleteFriendshipAsync(request.FriendTag, cancellationToken);
    }
}
