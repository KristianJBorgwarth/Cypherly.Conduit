using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Device.Queries.GetDevices;

public sealed class GetDevicesQueryHandler(
    IIdentityUserProvider identityUserProvider,
    IChatServerProvider chatServerProvider)
    : IQueryHandler<GetDevicesQuery, GetDevicesDto>
{
    public async ValueTask<Result<GetDevicesDto>> Handle(GetDevicesQuery request, CancellationToken cancellationToken)
    {
        var devicesTask = identityUserProvider.GetDevicesAsync(cancellationToken);
        var presenceTask = chatServerProvider.GetPresenceAsync(cancellationToken);

        var devices = await devicesTask;
        if (!devices.Success)
            return devices;

        var presence = await presenceTask;
        if (!presence.Success)
            return devices;

        var online = presence.RequiredValue.Devices.ToDictionary(d => d.DeviceId, d => d.Online);

        return Result.Ok(devices.RequiredValue with
        {
            Devices = [.. devices.RequiredValue.Devices.Select(d => d with { Online = online.TryGetValue(d.Id, out var isOnline) ? isOnline : null })],
        });
    }
}
