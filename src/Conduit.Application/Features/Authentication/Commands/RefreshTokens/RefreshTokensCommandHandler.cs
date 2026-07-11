using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Authentication.Commands.RefreshTokens;

public sealed class RefreshTokensCommandHandler(IIdentityProvider idProvider) : ICommandHandler<RefreshTokensCommand, RefreshTokensDto>
{
    public async Task<Result<RefreshTokensDto>> Handle(RefreshTokensCommand cmd, CancellationToken ct)
    {
        return await idProvider.RefreshTokensAsync(cmd.UserId, cmd.DeviceId, cmd.RefreshToken, ct);
    }
}
