using FluentValidation;

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;

public sealed class GetUserProfileByTagQueryValidator : AbstractValidator<GetUserProfileByTagQuery>
{
    public GetUserProfileByTagQueryValidator()
    {
        RuleFor(x => x.UserTag).NotEmpty();
    }
}