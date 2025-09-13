using Conduit.API.Common;
using Conduit.Application.Features.UserProfile.Commands.TogglePrivacy;
using Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;
using Conduit.Application.Features.UserProfile.Queries.GetUserProfile;
using Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Endpoints;

internal sealed class UserProfileEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/profile")
            .WithTags("user-profile")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        group.MapGet("/", async ([FromServices] ISender sender, [FromQuery] Guid exclusiveConnectionId) =>
            {
                var result = await sender.Send(new GetUserProfileQuery { ExclusiveConnectionId = exclusiveConnectionId });
                return result.Success ? Results.Ok(result.RequiredValue) : result.ToProblemDetails();
            })
            .Produces<GetUserProfileDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/tag", async ([FromServices] ISender sender, [FromQuery] string tag) =>
            {
                var result = await sender.Send(new GetUserProfileByTagQuery { UserTag = tag });
                if (!result.Success) return result.ToProblemDetails();
                
                return result.Value is not null ? Results.Ok(result.Value) : Results.NoContent();
            })
            .Produces<GetUserProfileByTagDto>()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        
        group.MapPost("profile-picture", async ([FromServices] ISender sender, [FromForm] IFormFile newProfilePicture, [FromQuery] Guid tenantId) =>
            {
                var result = await sender.Send(new UpdateProfilePictureCommand { NewProfilePicture = newProfilePicture });
                return result.Success ? Results.Ok(result.RequiredValue) : result.ToProblemDetails();
            })
            .Produces<UpdateProfilePictureDto>()
            .Accepts<IFormFile>("multipart/form-data")
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status415UnsupportedMediaType);

        group.MapPost("toggle-privacy", async ([FromServices] ISender sender, [FromBody] bool isPrivate) =>
            {
                var result = await sender.Send(new TogglePrivacyCommand { IsPrivate = isPrivate });
                return result.Success ? Results.Ok() : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}