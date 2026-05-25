using ICommand = Conduit.Application.Abstractions.ICommand;

namespace Conduit.Application.Features.UserProfile.Commands.TogglePrivacy;

public sealed record TogglePrivacyCommand : ICommand
{
    public bool IsPrivate { get; init; }
}