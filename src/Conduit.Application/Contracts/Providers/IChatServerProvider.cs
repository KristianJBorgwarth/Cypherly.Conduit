using Conduit.Application.Features.Device.Queries.GetDevices;
using Conduit.Domain.Common;

namespace Conduit.Application.Contracts.Providers;

public interface IChatServerProvider
{
    Task<Result<PresenceDto>> GetPresenceAsync(CancellationToken ct = default);
}
