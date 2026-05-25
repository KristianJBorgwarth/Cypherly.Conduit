using Conduit.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;

public sealed record UpdateProfilePictureCommand : ICommand<UpdateProfilePictureDto>
{
    public required IFormFile NewProfilePicture { get; init; }
}