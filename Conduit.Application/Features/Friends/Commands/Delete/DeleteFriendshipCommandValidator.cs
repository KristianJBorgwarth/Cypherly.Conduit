using FluentValidation;

namespace Conduit.Application.Features.Friends.Commands.Delete;

public sealed class DeleteFriendshipCommandValidator: AbstractValidator<DeleteFriendshipCommand>
{
    public DeleteFriendshipCommandValidator()
    {
        RuleFor(x => x.FriendTag)
            .NotEmpty()
            .MaximumLength(50);
    }
}