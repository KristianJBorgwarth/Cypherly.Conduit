using Conduit.Application.Features.Authentication.Commands.Login;
using Conduit.Application.Features.Authentication.Commands.RefreshTokens;
using Conduit.Application.Features.Authentication.Commands.VerifyLogin;
using Conduit.Application.Features.Authentication.Commands.VerifyNonce;
using Conduit.Application.Features.Authentication.Queries;
using Conduit.Domain.Common;

namespace Conduit.Application.Contracts.Providers;

public interface IIdentityProvider
{
    public Task<Result<LoginDto>> LoginAsync(string email, string password, CancellationToken ct = default);
    public Task<Result> LogoutAsync(CancellationToken ct = default);
    public Task<Result<VerifyLoginDto>> VerifyLoginAsync(Guid userId, string code, CancellationToken ct = default);
    public Task<Result<GetNonceDto>> GetNonceAsync(Guid userId, Guid deviceId, CancellationToken ct = default);
    public Task<Result<VerifyNonceDto>> VerifyNonceAsync(Guid userId, Guid nonceId, Guid deviceId, string nonce, CancellationToken ct = default);
    public Task<Result<RefreshTokensDto>> RefreshTokensAsync(Guid userId, Guid deviceId, string refreshToken, CancellationToken ct = default);
}
