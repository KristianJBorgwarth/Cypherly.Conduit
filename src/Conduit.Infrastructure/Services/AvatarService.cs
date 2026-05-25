using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Caching.Hybrid;

internal sealed class AvatarService(
    HybridCache cache,
    IUserProfileSettingsProvider uspProvider) 
    : IAvatarService
{
    private readonly HybridCacheEntryOptions _cacheEntryOptions = new()
    {
        Expiration = TimeSpan.FromHours(2),
        LocalCacheExpiration = TimeSpan.FromMinutes(30)
    };
    public async Task<Result<Avatar>> GetAvatarAsync(Guid fileKey, CancellationToken ct = default)
    {
        var avatar = await cache.GetOrCreateAsync(
            $"avatar-{fileKey}",
            async entry =>
            {
                return await uspProvider.GetAvatar(fileKey, ct);
            },
            _cacheEntryOptions,
            cancellationToken: ct);

        return avatar;
    }
}
