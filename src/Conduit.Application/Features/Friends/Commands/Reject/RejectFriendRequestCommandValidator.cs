using FluentValidation;

namespace Conduit.Application.Features.Friends.Commands.Reject;

public sealed class RejectFriendRequestCommandValidator: AbstractValidator<RejectFriendRequestCommand>
{
    public RejectFriendRequestCommandValidator()
    {
        RuleFor(x => x.FriendTag)
            .NotEmpty()
            .MaximumLength(50);
    }
}