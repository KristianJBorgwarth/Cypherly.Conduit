using Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;
using Conduit.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Conduit.Application.Contracts.Providers;

public interface IUserProfileSettingsProvider
{
    public Task<Result<UpdateProfilePictureDto>> UpdateProfilePicture(IFormFile newProfilePicture, CancellationToken ct = default);
    public Task<Result> ToggleProfilePrivacyAsync(bool isPrivate, CancellationToken ct = default);
}