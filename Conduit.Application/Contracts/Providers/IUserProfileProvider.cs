using Conduit.Application.Features.UserProfile.Commands.UpdateDisplayName;
using Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;
using Conduit.Domain.Common;
using Conduit.Domain.Models;

namespace Conduit.Application.Contracts.Providers;

public interface IUserProfileProvider
{
    public Task<Result<UserProfile>> GetUserProfile(Guid exclusiveConnectionId, CancellationToken ct = default);
    public Task<Result<GetUserProfileByTagDto>> GetUserProfileByTag(string userTag, CancellationToken ct = default);
    public Task<Result<string>> UpdateDisplayName(string newDisplayName, CancellationToken ct = default);
}
