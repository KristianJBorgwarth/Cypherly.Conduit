using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Account.Commands.Register;

public sealed record RegisterCommand : ICommand<RegisterDto>
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
