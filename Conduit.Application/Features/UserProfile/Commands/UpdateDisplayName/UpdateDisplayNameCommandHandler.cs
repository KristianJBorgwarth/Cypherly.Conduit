using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateDisplayName;

public sealed class UpdateDisplayNameCommandHandler(
        IUserProfileProvider userProfileProvider,
        ILogger<UpdateDisplayNameCommandHandler> logger)
    : ICommandHandler<UpdateDisplayNameCommand, UpdateDisplayNameDto>
{
    public async Task<Result<UpdateDisplayNameDto>> Handle(UpdateDisplayNameCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await userProfileProvider.UpdateDisplayName(request.NewDisplayName, cancellationToken);

            return result.Success is true ? new UpdateDisplayNameDto { DisplayName = result.RequiredValue }: Result.Fail<UpdateDisplayNameDto>(result.Error!);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while updating display name.");
            return Result.Fail<UpdateDisplayNameDto>(Error.Failure("An error occurred while updating display name."));
        }
    }
}
