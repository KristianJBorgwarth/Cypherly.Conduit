using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Account.Commands.ResendVerificationCode;

public sealed record ResendVerificationCodeCommand : ICommand
{
    public required Guid UserId { get; init; }
    public required int CodeType { get; init; }
    public Guid? NonceId { get; init; }
    public string? Nonce { get; init; }
}
