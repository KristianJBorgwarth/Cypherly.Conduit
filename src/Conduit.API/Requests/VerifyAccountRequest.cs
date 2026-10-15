namespace Conduit.API.Requests;

public sealed record VerifyAccountRequest
{
    public required Guid UserId { get; init; }
    public required string VerificationCode { get; init; }
}
