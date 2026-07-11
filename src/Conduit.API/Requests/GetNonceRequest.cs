namespace Conduit.API.Requests;

public sealed record GetNonceRequest
{
    public required Guid UserId { get; init; }
    public required Guid DeviceId { get; init; }
}
