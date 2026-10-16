using FluentValidation;

namespace Conduit.Application.Features.Friends.Commands.Accept;

public sealed class AcceptFriendshipCommandValidator : AbstractValidator<AcceptFriendshipCommand>
{
    public AcceptFriendshipCommandValidator()
    {
        RuleFor(x=> x.FriendTag)
            .NotEmpty();
    }
}