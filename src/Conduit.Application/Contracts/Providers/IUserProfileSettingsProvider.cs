using Conduit.Application.Features.UserProfile.Commands.UpdateAvatar;
using Conduit.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Conduit.Application.Contracts.Providers;

public interface IUserProfileSettingsProvider
{
    public Task<Result<UpdateAvatarDto>> UpdateAvatar(IFormFile avatar, CancellationToken ct = default);
    public Task<Result<Avatar>> GetAvatar(Guid fileKey, CancellationToken ct = default);
    public Task<Result> TogglePrivacy(bool isPrivate, CancellationToken ct = default);
}
