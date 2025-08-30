using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Social.Queries;

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
            var userProfile = await userProfileProvider.GetUserProfile(request.ExclusiveConnectionId, cancellationToken);
            if (userProfile is null)
                return Result.Fail<GetUserProfileDto>(Error.NotFound("UserProfile not found"));

            var connectionIds = await connectionIdProvider.GetConnectionIds(cancellationToken);

            connectionIds = RemoveConnectionId(connectionIds, request.ExclusiveConnectionId);
            
            var dto = new GetUserProfileDto(userProfile, connectionIds);
            return Result.Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occured while retrieving user profile for Tenant");
            return Result.Fail<GetUserProfileDto>(Error.NotFound("UserProfile not found"));
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