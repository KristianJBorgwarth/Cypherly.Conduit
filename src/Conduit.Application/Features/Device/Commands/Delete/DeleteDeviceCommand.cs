using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Device.Commands.Delete;

public sealed record DeleteDeviceCommand : ICommand
{
    public required Guid TargetDeviceId { get; init; }
}
