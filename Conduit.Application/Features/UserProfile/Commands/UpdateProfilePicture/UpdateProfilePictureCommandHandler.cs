using Conduit.Application.Abstractions;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;

public sealed class UpdateProfilePictureCommandHandler : ICommandHandler<UpdateProfilePictureCommand, UpdateProfilePictureDto>
{
    public Task<Result<UpdateProfilePictureDto>> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}