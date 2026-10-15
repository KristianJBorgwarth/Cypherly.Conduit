namespace Conduit.API.Requests;

public sealed record ResendVerificationCodeRequest
{
    public required Guid UserId { get; init; }
    public required int CodeType { get; init; }
    public Guid? NonceId { get; init; }
    public string? Nonce { get; init; }
}
