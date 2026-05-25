using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Authentication.Commands.Logout;

public sealed class LogoutCommandHandler(IIdentityProvider identityProvider) : ICommandHandler<LogoutCommand>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await identityProvider.LogoutAsync(cancellationToken);
    }
}
