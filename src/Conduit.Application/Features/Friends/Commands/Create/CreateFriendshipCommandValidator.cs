using FluentValidation;

namespace Conduit.Application.Features.Friends.Commands.Create;

public sealed class CreateFriendshipCommandValidator : AbstractValidator<CreateFriendshipCommand>
{
    public CreateFriendshipCommandValidator()
    {
        RuleFor(x=> x.FriendTag)
            .NotEmpty();
    }
}