namespace Conduit.API.Requests;

public sealed record RefreshTokensRequest
{
    public required Guid UserId { get; init; }
    public required Guid DeviceId { get; init; }
    public required string RefreshToken { get; init; }
}
