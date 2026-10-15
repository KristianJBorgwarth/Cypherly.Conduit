using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Account.Commands.Verify;

public sealed record VerifyAccountCommand : ICommand
{
    public required Guid UserId { get; init; }
    public required string VerificationCode { get; init; }
}
