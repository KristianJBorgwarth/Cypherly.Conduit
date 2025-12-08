using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;

public sealed class GetUserProfileByTagQueryHandler(IUserProfileProvider userProfileProvider) : IQueryHandler<GetUserProfileByTagQuery, GetUserProfileByTagDto>
{
    public async Task<Result<GetUserProfileByTagDto>> Handle(GetUserProfileByTagQuery request, CancellationToken ct)
    {
        var profileResult = await userProfileProvider.GetUserProfileByTag(request.UserTag, ct);
        return profileResult.Success ? Result.Ok(profileResult.Value) : Result.Fail<GetUserProfileByTagDto>(profileResult.Error);
    }
}
