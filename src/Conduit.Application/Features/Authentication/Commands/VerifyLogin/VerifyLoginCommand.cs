using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Authentication.Commands.VerifyLogin;

public sealed record VerifyLoginCommand : ICommand<VerifyLoginDto>
{
    public required string LoginVerificationCode { get; init; }
}
