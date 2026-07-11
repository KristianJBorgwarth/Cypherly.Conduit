using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Authentication.Commands.RefreshTokens;

public sealed record RefreshTokensCommand : ICommand<RefreshTokensDto>
{
    public required Guid UserId { get; init; }
    public required Guid DeviceId { get; init; }
    public required string RefreshToken { get; init; }
}
