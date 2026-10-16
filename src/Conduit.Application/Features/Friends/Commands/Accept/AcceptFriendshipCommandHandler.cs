using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Friends.Commands.Accept;

public sealed class AcceptFriendshipCommandHandler(IFriendProvider friendProvider) : ICommandHandler<AcceptFriendshipCommand, AcceptFriendshipDto>
{
    public async ValueTask<Result<AcceptFriendshipDto>> Handle(AcceptFriendshipCommand request, CancellationToken cancellationToken)
    {
        return await friendProvider.AcceptFriendshipAsync(request.FriendTag, cancellationToken);
    }
}
