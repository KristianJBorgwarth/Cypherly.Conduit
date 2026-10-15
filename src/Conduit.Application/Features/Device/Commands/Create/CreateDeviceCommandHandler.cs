using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Device.Commands.Create;

public sealed class CreateDeviceCommandHandler(IIdentityUserProvider identityUserProvider) : ICommandHandler<CreateDeviceCommand, CreateDeviceDto>
{
    public async ValueTask<Result<CreateDeviceDto>> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        return await identityUserProvider.CreateDeviceAsync(
            request.UserId,
            request.LoginNonceId,
            request.LoginNonce,
            request.DeviceAppVersion,
            request.DeviceType,
            request.DevicePlatform,
            request.Base64DevicePublicKey,
            cancellationToken);
    }
}
