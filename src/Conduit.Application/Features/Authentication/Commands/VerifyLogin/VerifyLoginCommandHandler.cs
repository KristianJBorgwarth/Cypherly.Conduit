using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Authentication.Commands.VerifyLogin;

public sealed class VerifyLoginCommandHandler(IIdentityProvider idProvider) : ICommandHandler<VerifyLoginCommand, VerifyLoginDto>
{
    public async ValueTask<Result<VerifyLoginDto>> Handle(VerifyLoginCommand cmd, CancellationToken ct)
    {
        return await idProvider.VerifyLoginAsync(cmd.UserId, cmd.LoginVerificationCode, cmd.NonceId, cmd.Nonce, ct);
    }
}
