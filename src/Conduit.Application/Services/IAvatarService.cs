using Conduit.Domain.Common;

public interface IAvatarService
{
    Task<Result<Avatar>> GetAvatarAsync(Guid fileKey, CancellationToken ct = default);
}
