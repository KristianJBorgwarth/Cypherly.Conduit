using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Authentication.Commands.VerifyNonce;

public sealed record VerifyNonceCommand : ICommand<VerifyNonceDto>
{
    public required Guid UserId { get; init; }
    public required Guid NonceId { get; init; }
    public required Guid DeviceId { get; init; }
    public required string Nonce { get; init; }
}
