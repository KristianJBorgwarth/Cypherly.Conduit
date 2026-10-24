using System.Security.Claims;

namespace Conduit.API.Common;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetDeviceId(this ClaimsPrincipal principal)
    {
        var deviceId = principal.FindFirst("device_id");
        return Guid.TryParse(deviceId?.Value, out var parsedGuid)
            ? parsedGuid
            : throw new UnauthorizedAccessException("Device ID not found or invalid.");
    }
}
