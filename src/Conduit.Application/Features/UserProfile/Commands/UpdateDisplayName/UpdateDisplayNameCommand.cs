using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateDisplayName;

public sealed record UpdateDisplayNameCommand : ICommand<UpdateDisplayNameDto>
{
    public required string NewDisplayName { get; init; }
}
