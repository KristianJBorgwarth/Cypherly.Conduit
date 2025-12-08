using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;

public sealed class UpdateProfilePictureCommandHandler(IUserProfileSettingsProvider userProfileSettingsProvider) : ICommandHandler<UpdateProfilePictureCommand, UpdateProfilePictureDto>
{
    public async Task<Result<UpdateProfilePictureDto>> Handle(UpdateProfilePictureCommand request,
        CancellationToken cancellationToken)
    {
        return await userProfileSettingsProvider.UpdateProfilePicture(request.NewProfilePicture, cancellationToken);
    }
}
