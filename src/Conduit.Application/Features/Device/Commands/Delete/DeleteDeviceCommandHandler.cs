using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Device.Commands.Delete;

public sealed class DeleteDeviceCommandHandler(IIdentityUserProvider identityUserProvider) : ICommandHandler<DeleteDeviceCommand>
{
    public async ValueTask<Result> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
    {
        return await identityUserProvider.DeleteDeviceAsync(request.TargetDeviceId, cancellationToken);
    }
}
