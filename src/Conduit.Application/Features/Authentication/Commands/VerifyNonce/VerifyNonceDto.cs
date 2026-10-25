namespace Conduit.Application.Features.Authentication.Commands.VerifyNonce;

public sealed record VerifyNonceDto
{
    public required string Jwt { get; init; }
    public DateTime ExpiresAt { get; init; }
    public IReadOnlyList<string> TokenResponses { get; init; } = [];
}
