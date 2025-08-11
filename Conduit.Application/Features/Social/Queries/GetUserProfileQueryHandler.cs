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
    public async Task<Result<GetUserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userProfile = await userProfileProvider.GetUserProfile(cancellationToken);
            if (userProfile is null) 
                return Result.Fail<GetUserProfileDto>(Error.NotFound("UserProfile not found"));
            
            var connectionIds = await connectionIdProvider.GetConnectionIds(cancellationToken);
            
            var dto = new GetUserProfileDto(userProfile, connectionIds);
            return Result.Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occured while retrieving user profile for Tenant");
            return Result.Fail<GetUserProfileDto>(Error.NotFound("UserProfile not found"));
        }
    }
}