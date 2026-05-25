using FluentValidation;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;

public sealed class UpdateProfilePictureCommandValidator : AbstractValidator<UpdateProfilePictureCommand>
{
    public UpdateProfilePictureCommandValidator()
    {
        RuleFor(cmd => cmd.NewProfilePicture)
            .NotNull().NotEmpty();
    }
}