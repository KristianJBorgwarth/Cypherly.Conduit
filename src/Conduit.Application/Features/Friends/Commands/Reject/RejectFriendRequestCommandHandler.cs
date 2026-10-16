using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Friends.Commands.Reject;

public sealed class RejectFriendRequestCommandHandler(IFriendProvider friendProvider) : ICommandHandler<RejectFriendRequestCommand>
{
    public async ValueTask<Result> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
    {
        return await friendProvider.RejectFriendRequestAsync(request.FriendTag, cancellationToken);
    }
}
