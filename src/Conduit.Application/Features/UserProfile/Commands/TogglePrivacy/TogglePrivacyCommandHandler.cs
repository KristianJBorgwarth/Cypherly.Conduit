using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Commands.TogglePrivacy;

public sealed class TogglePrivacyCommandHandler(IUserProfileSettingsProvider userProfileSettingsProvider) : ICommandHandler<TogglePrivacyCommand>
{
    public ValueTask<Result> Handle(TogglePrivacyCommand request, CancellationToken cancellationToken)
    {
        return new ValueTask<Result>(userProfileSettingsProvider.TogglePrivacy(request.IsPrivate, cancellationToken));
    }
}
