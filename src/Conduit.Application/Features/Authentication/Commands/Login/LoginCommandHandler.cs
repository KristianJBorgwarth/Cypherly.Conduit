using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Authentication.Commands.Login;

public sealed class LoginCommandHandler(IIdentityProvider identityProvider) : ICommandHandler<LoginCommand, LoginDto>
{
    public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return await identityProvider.LoginAsync(request.Email, request.Password, cancellationToken);
    }
}
