using Conduit.Application.Abstractions;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Queries.GetAvatar
{
    public sealed class GetAvatarQueryHandler(
        IAvatarService avatarService)
        : IQueryHandler<GetAvatarQuery, Avatar>
    {
        public Task<Result<Avatar>> Handle(GetAvatarQuery q, CancellationToken ct)
        {
            return avatarService.GetAvatarAsync(q.FileKey, ct);
        }
    }
}
