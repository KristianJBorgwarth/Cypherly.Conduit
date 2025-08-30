using Conduit.Application.Features.Social.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Endpoints;

public sealed class UserProfileEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/profile")
            .WithTags("user-profile")
            .RequireAuthorization();

        group.MapGet("/", async ([FromServices]ISender sender, [FromQuery] Guid exclusiveConnectionId) =>
        {
            var result = await sender.Send(new GetUserProfileQuery { ExclusiveConnectionId = exclusiveConnectionId });
            return result.Success ? Results.Ok(result.Value) : Results.BadRequest();
        });
    }
}