using Conduit.Application.Features.Social.Queries;
using MediatR;

namespace Conduit.API.Endpoints;

public sealed class UserProfileEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/profile")
            .WithTags("user-profile")
            .RequireAuthorization();

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new GetUserProfileQuery());
            return result.Success ? Results.Ok(result.Value) : Results.BadRequest();
        });
    }
}