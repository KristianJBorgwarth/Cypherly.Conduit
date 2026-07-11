namespace Conduit.Application.Features.Authentication.Commands.VerifyLogin;

public sealed record VerifyLoginDto
{
    public required Guid NonceId { get; init; }
    public required string Nonce { get; init; }
}
