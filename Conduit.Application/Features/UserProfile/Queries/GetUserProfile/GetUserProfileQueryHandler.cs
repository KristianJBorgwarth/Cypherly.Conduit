using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfile;

public sealed class GetUserProfileQueryHandler(
    IUserProfileProvider userProfileProvider,
    IConnectionIdProvider connectionIdProvider)
    : IQueryHandler<GetUserProfileQuery, GetUserProfileDto>
{
    public async Task<Result<GetUserProfileDto>> Handle(GetUserProfileQuery request,
        CancellationToken cancellationToken)
    {
        var profileResult = await userProfileProvider.GetUserProfile(cancellationToken);
        if (!profileResult.Success) return Result.Fail<GetUserProfileDto>(profileResult.Error);

        var conIdsResult = await connectionIdProvider.GetConnectionIds(cancellationToken);
        if (!conIdsResult.Success) return Result.Fail<GetUserProfileDto>(conIdsResult.Error);

        var connectionIds = RemoveConnectionId(conIdsResult.RequiredValue, request.ExclusiveConnectionId);

        var dto = new GetUserProfileDto(profileResult.RequiredValue, connectionIds);
        return Result.Ok(dto);
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
        [.. connectionIds.Where(id => id != exclusiveConnectionId)];
}
