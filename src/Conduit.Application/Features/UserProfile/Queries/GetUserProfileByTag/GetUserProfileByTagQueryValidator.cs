using FluentValidation;

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;

public sealed class GetUserProfileByTagQueryValidator : AbstractValidator<GetUserProfileByTagQuery>
{
    public GetUserProfileByTagQueryValidator()
    {
        RuleFor(x => x.UserTag)
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("User tag must not be empty or whitespace.")
            .MaximumLength(20);
    }
}