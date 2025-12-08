using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Friends.Commands.Create;

public sealed class CreateFriendshipCommandHandler(IFriendProvider friendProvider) : ICommandHandler<CreateFriendshipCommand>
{
    public async Task<Result> Handle(CreateFriendshipCommand request, CancellationToken cancellationToken)
    {
        return await friendProvider.CreateFriendshipAsync(request.FriendTag, cancellationToken);
    }
}
