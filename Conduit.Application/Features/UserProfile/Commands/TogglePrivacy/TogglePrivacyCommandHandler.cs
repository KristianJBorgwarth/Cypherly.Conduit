using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Commands.TogglePrivacy;

public sealed class TogglePrivacyCommandHandler(IUserProfileSettingsProvider userProfileSettingsProvider) : ICommandHandler<TogglePrivacyCommand>
{
    public Task<Result> Handle(TogglePrivacyCommand request, CancellationToken cancellationToken)
    {
        return userProfileSettingsProvider.ToggleProfilePrivacyAsync(request.IsPrivate, cancellationToken);
    }
}
