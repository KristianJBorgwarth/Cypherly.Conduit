using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Authentication.Commands.Logout;

public sealed record LogoutCommand : ICommand
{
    public Guid DeviceId { get; init; }
}
