using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateAvatar;

public sealed class UpdateAvatarCommandHandler(
    IUserProfileSettingsProvider upsp) 
    : ICommandHandler<UpdateAvatarcommand, UpdateAvatarDto>
{
    public async Task<Result<UpdateAvatarDto>> Handle(UpdateAvatarcommand cmd, CancellationToken ct) 
        => await upsp.UpdateAvatar(cmd.Avatar, ct);
}
