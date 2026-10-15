namespace Conduit.Application.Features.Device.Commands.Create;

public sealed record CreateDeviceDto
{
    public required Guid DeviceId { get; init; }
    public required Guid DeviceConnectionId { get; init; }
}
