using Conduit.Application.Features.Authentication.Commands.Login;
using Conduit.Application.Features.Authentication.Commands.VerifyLogin;
using Conduit.Domain.Common;

namespace Conduit.Application.Contracts.Providers;

public interface IIdentityProvider
{
    public Task<Result<LoginDto>> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task<Result> LogoutAsync(CancellationToken cancellationToken = default);
    public Task<Result<VerifyLoginDto>> VerifyLoginAsync(string loginVerificationCode, CancellationToken cancellationToken = default);
}
