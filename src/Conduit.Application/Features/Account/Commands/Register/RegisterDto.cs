namespace Conduit.Application.Features.Account.Commands.Register;

public sealed record RegisterDto
{
    public required Guid Id { get; init; }
    public required string Email { get; init; }
}
