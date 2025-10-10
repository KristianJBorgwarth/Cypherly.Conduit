using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Authentication.Commands.Login;

public sealed class LoginCommandHandler (
    IIdentityProvider identityProvider, 
    ILogger<LoginCommandHandler> logger)
    : ICommandHandler<LoginCommand, LoginDto>
{
    public async Task<Result<LoginDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await identityProvider.LoginAsync(request.Email, request.Password, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing LoginCommand"); 
            return Result.Fail<LoginDto>(Error.Failure("An unexpected error occurred."));
        }
    }
}
