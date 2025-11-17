using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Authentication.Commands.Logout;

public sealed class LogoutCommandHandler(
    IIdentityProvider identityProvider,
    ILogger<LogoutCommandHandler> logger)
  : ICommandHandler<LogoutCommand>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await identityProvider.LogoutAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while logging out device");
            return Result.Fail(Error.Failure("An unexpected error occurred during logout."));
        }
    }
}
