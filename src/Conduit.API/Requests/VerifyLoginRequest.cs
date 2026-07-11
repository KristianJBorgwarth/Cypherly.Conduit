namespace Conduit.API.Requests;

public sealed record VerifyLoginRequest
{
    public required Guid UserId { get; init; }
    public required string LoginVerificationCode { get; init; }
}
