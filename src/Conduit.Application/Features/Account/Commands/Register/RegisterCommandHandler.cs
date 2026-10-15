using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Account.Commands.Register;

public sealed class RegisterCommandHandler(IIdentityUserProvider identityUserProvider) : ICommandHandler<RegisterCommand, RegisterDto>
{
    public async ValueTask<Result<RegisterDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        return await identityUserProvider.RegisterAsync(request.Username, request.Email, request.Password, cancellationToken);
    }
}
