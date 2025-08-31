using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;

public sealed class GetUserProfileByTagQueryHandler(
    IUserProfileProvider userProfileProvider,
    ILogger<GetUserProfileByTagQueryHandler> logger)
    : IQueryHandler<GetUserProfileByTagQuery, GetUserProfileByTagDto>
{
    public async Task<Result<GetUserProfileByTagDto>> Handle(GetUserProfileByTagQuery request, CancellationToken ct)
    {
        try
        {
            var userProfile = await userProfileProvider.GetUserProfileByTag(request.UserTag, ct);
            return userProfile == null ? Result.Ok<GetUserProfileByTagDto>() : Result.Ok(userProfile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting user profile by tag: {Message}", ex.Message);
            return Result.Fail<GetUserProfileByTagDto>(Error.Failure("internal.server.error", "An exception occurred while processing the request."));
        }
    }
}