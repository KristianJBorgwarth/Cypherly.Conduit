using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;

public sealed class UpdateProfilePictureCommandHandler(
    IUserProfileSettingsProvider userProfileSettingsProvider,
    ILogger<UpdateProfilePictureCommandHandler> logger)
    : ICommandHandler<UpdateProfilePictureCommand, UpdateProfilePictureDto>
{
    public async Task<Result<UpdateProfilePictureDto>> Handle(UpdateProfilePictureCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await userProfileSettingsProvider.UpdateProfilePicture(request.NewProfilePicture, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred while updating profile picture");
            return Result.Fail<UpdateProfilePictureDto>(Error.Failure("An error occurred while processing the request."));
        }
    }
}
