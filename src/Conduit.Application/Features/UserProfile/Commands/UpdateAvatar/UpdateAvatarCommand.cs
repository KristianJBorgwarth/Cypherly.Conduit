using Conduit.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateAvatar;

public sealed record UpdateAvatarcommand : ICommand<UpdateAvatarDto>
{
    public required IFormFile Avatar { get; init; }
}
