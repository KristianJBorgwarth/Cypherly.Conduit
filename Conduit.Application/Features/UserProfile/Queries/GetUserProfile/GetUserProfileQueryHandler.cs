using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfile;

public sealed class GetUserProfileQueryHandler(
    IUserProfileProvider userProfileProvider,
    IConnectionIdProvider connectionIdProvider,
    ILogger<GetUserProfileQueryHandler> logger)
    : IQueryHandler<GetUserProfileQuery, GetUserProfileDto>
{
    public async Task<Result<GetUserProfileDto>> Handle(GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var profileResult = await userProfileProvider.GetUserProfile(request.ExclusiveConnectionId, cancellationToken);
            if (!profileResult.Success) return Result.Fail<GetUserProfileDto>(profileResult.Error);

            var connectionIdsResult = await connectionIdProvider.GetConnectionIds(cancellationToken);
            if (!connectionIdsResult.Success) return Result.Fail<GetUserProfileDto>(connectionIdsResult.Error);
            
            var connectionIds = RemoveConnectionId(connectionIdsResult.Value, request.ExclusiveConnectionId);

            var dto = new GetUserProfileDto(profileResult.Value, connectionIds);
            return Result.Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occured while retrieving user profile for Tenant");
            return Result.Fail<GetUserProfileDto>(Error.Failure("internal.server.error", "An unexpected error occured"));
        }
    }

    /// <summary>
    /// Exclude the current connection id from the list of connection ids
    /// to avoid sending the id of the requesting client back to itself.
    /// </summary>
    /// <param name="connectionIds">ids to modify</param>
    /// <param name="exclusiveConnectionId">the ids to filter out</param>
    /// <returns></returns>
    private static List<Guid> RemoveConnectionId(IReadOnlyCollection<Guid> connectionIds,
        Guid exclusiveConnectionId) =>
        connectionIds.Where(id => id != exclusiveConnectionId).ToList();
}