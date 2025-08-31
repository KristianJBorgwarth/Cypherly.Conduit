using Conduit.Application.Features.UserProfile.Queries.GetUserProfile;
using Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Endpoints;

public sealed class UserProfileEndpoints : IEndpoint
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
                return result.Success ? Results.Ok(result.Value) : Results.BadRequest();
            })
            .Produces<GetUserProfileDto>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/tag", async ([FromServices] ISender sender, [FromQuery] string tag) =>
            {
                var result = await sender.Send(new GetUserProfileByTagQuery { UserTag = tag });
                return result.Success ? Results.Ok(result.Value) : Results.BadRequest();
            })
            .Produces<GetUserProfileByTagDto>()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}