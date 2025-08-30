using Conduit.Domain.Models;

namespace Conduit.Application.Contracts.Providers;

public interface IUserProfileProvider
{
    public Task<UserProfile?> GetUserProfile(Guid exclusiveConnectionId, CancellationToken cancellationToken = default);
}