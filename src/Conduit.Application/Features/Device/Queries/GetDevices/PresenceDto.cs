namespace Conduit.Application.Features.Device.Queries.GetDevices;

public sealed record PresenceDto
{
    public required IReadOnlyCollection<DevicePresenceDto> Devices { get; init; } = [];
}

public sealed record DevicePresenceDto
{
    public required Guid DeviceId { get; init; }
    public required bool Online { get; init; }
}
