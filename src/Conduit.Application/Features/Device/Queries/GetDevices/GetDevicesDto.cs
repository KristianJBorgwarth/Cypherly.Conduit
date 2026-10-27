namespace Conduit.Application.Features.Device.Queries.GetDevices;

public sealed record GetDevicesDto
{
    public required IReadOnlyCollection<DeviceDto> Devices { get; init; } = [];
}

public sealed record DeviceDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Platform { get; init; }
    public DateTime? LastSeen { get; init; }
    public bool? Online { get; init; }
}
