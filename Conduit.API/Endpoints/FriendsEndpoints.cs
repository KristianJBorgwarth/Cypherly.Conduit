using Conduit.Application.Features.Friends.Queries.GetFriends;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Endpoints;

public sealed class FriendsEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/friend")
            .WithTags("user-profile")
            .RequireAuthorization();
        
        group.MapGet("/list", async ([FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetFriendsQuery());
            if (!result.Success) return Results.BadRequest();
            return result.Value?.Count > 0 ? Results.Ok(result.Value) : Results.NoContent();
        });
    }
}