using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Device.Commands.Create;

public sealed record CreateDeviceCommand : ICommand<CreateDeviceDto>
{
    public required Guid UserId { get; init; }
    public required Guid LoginNonceId { get; init; }
    public required string LoginNonce { get; init; }
    public required string DeviceAppVersion { get; init; }
    public required int DeviceType { get; init; }
    public required int DevicePlatform { get; init; }
    public required string Base64DevicePublicKey { get; init; }
}
