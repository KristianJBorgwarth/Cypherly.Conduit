using FluentValidation;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateAvatar;

public sealed class UpdateProfilePictureCommandValidator : AbstractValidator<UpdateAvatarcommand>
{
    public UpdateProfilePictureCommandValidator()
    {
        RuleFor(cmd => cmd.Avatar)
            .NotNull().NotEmpty();
    }
}
