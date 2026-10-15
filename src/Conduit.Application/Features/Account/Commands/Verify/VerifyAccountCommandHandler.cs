using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Account.Commands.Verify;

public sealed class VerifyAccountCommandHandler(IIdentityUserProvider identityUserProvider) : ICommandHandler<VerifyAccountCommand>
{
    public async ValueTask<Result> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
    {
        return await identityUserProvider.VerifyAsync(request.UserId, request.VerificationCode, cancellationToken);
    }
}
