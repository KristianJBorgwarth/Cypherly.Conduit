using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Authentication.Commands.VerifyNonce;

public sealed class VerifyNonceCommandHandler(IIdentityProvider idProvider) : ICommandHandler<VerifyNonceCommand, VerifyNonceDto>
{
    public async Task<Result<VerifyNonceDto>> Handle(VerifyNonceCommand cmd, CancellationToken ct)
    {
        return await idProvider.VerifyNonceAsync(cmd.UserId, cmd.NonceId, cmd.DeviceId, cmd.Nonce, ct);
    }
}
