public sealed record VerifyLoginRequest
{
    public required string LoginVerificationCode { get; init; }
}
