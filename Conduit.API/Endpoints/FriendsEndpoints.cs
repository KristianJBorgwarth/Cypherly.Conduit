using Conduit.API.Common;
using Conduit.API.Requests;
using Conduit.Application.Features.Friends.Commands.Create;
using Conduit.Application.Features.Friends.Queries.GetFriendRequests;
using Conduit.Application.Features.Friends.Queries.GetFriends;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace Conduit.API.Endpoints;

public sealed class FriendsEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/friend")
            .WithTags("friend")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized);
        
        group.MapGet("/list", async ([FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetFriendsQuery());
            if (!result.Success) return result.ToProblemDetails();
            return result.Value?.Count > 0 ? Results.Ok(result.Value) : Results.NoContent();
        });

        group.MapPost("", async ([FromServices] ISender sender, [FromBody] CreateFriendshipRequest req) =>
            {
                var result = await sender.Send(new CreateFriendshipCommand { FriendTag = req.FriendTag });
                return result.Success ? Results.Ok() : result.ToProblemDetails();
            })
            .Accepts<CreateFriendshipRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        
        group.MapGet("/requests", async ([FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetFriendRequestsQuery());
            if (!result.Success) return result.ToProblemDetails();
            return result.Value?.Count > 0 ? Results.Ok(result.Value) : Results.NoContent();
        })
        .Produces<IReadOnlyCollection<GetFriendRequestsDto>>()
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}