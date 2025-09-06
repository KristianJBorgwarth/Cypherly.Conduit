using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.UserProfile.Commands.TogglePrivacy;

public sealed class TogglePrivacyCommandHandler(
    IUserProfileSettingsProvider userProfileSettingsProvider,
    ILogger<TogglePrivacyCommandHandler> logger) 
    : ICommandHandler<TogglePrivacyCommand>
{
    public Task<Result> Handle(TogglePrivacyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return userProfileSettingsProvider.ToggleProfilePrivacyAsync(request.IsPrivate, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred while toggling profile privacy");
            return Task.FromResult(Result.Fail(Error.Failure("An error occurred while processing the request.")));
        }
    }
}