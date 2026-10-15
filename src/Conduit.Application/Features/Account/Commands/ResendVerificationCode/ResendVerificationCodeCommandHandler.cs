using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Account.Commands.ResendVerificationCode;

public sealed class ResendVerificationCodeCommandHandler(IIdentityUserProvider identityUserProvider) : ICommandHandler<ResendVerificationCodeCommand>
{
    public async ValueTask<Result> Handle(ResendVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        return await identityUserProvider.ResendVerificationCodeAsync(request.UserId, request.CodeType, request.NonceId, request.Nonce, cancellationToken);
    }
}
