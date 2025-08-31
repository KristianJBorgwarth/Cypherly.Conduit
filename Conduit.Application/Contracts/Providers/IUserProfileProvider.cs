using Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;
using Conduit.Domain.Common;
using Conduit.Domain.Models;

namespace Conduit.Application.Contracts.Providers;

public interface IUserProfileProvider
{
    public Task<Result<UserProfile>> GetUserProfile(Guid exclusiveConnectionId, CancellationToken cancellationToken = default);
    public Task<GetUserProfileByTagDto?> GetUserProfileByTag(string userTag, CancellationToken cancellationToken = default);
}