using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Queries.GetAvatar
{
    public sealed class GetAvatarQueryHandler(
        IUserProfileSettingsProvider uspProvider)
        : IQueryHandler<GetAvatarQuery, Avatar>
    {
        public ValueTask<Result<Avatar>> Handle(GetAvatarQuery q, CancellationToken ct) => new(uspProvider.GetAvatar(q.FileKey, ct));
    }
}
