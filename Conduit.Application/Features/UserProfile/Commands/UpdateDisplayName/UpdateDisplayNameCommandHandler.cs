using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateDisplayName;

public sealed class UpdateDisplayNameCommandHandler(IUserProfileProvider userProfileProvider) : ICommandHandler<UpdateDisplayNameCommand, UpdateDisplayNameDto>
{
    public async Task<Result<UpdateDisplayNameDto>> Handle(UpdateDisplayNameCommand request, CancellationToken cancellationToken)
    {
        var result = await userProfileProvider.UpdateDisplayName(request.NewDisplayName, cancellationToken);
        return result.Success is true ? new UpdateDisplayNameDto { DisplayName = result.RequiredValue } : Result.Fail<UpdateDisplayNameDto>(result.Error);
    }
}
