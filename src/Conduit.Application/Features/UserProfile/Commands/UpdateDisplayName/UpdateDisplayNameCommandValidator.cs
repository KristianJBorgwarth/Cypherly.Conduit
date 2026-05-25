using FluentValidation;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateDisplayName;

public sealed class UpdateDisplayNameCommandValidator : AbstractValidator<UpdateDisplayNameCommand>
{
    public UpdateDisplayNameCommandValidator()
    {
        RuleFor(x => x.NewDisplayName)
            .NotEmpty()
            .MaximumLength(50);
    }
}
