namespace Conduit.API.Requests;

public sealed record VerifyNonceRequest
{
    public required Guid UserId { get; init; }
    public required Guid NonceId { get; init; }
    public required Guid DeviceId { get; init; }
    public required string Nonce { get; init; }
}
