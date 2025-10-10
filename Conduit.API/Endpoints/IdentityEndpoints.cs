using Conduit.API.Common;
using Conduit.API.Requests;
using Conduit.Application.Features.Authentication.Commands.Login;
using Conduit.Application.Features.Authentication.Commands.Logout;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Endpoints;

internal sealed class IdentityEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/identity")
            .WithTags("identity");

        group.MapPost("login", async ([FromServices] ISender sender, [FromBody] LoginRequest req) =>
        {
            var result = await sender.Send(new LoginCommand { Email = req.Email, Password = req.Password });
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<LoginDto>()
        .Accepts<LoginRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("logout", async ([FromServices] ISender sender, [FromBody] Guid deviceId) =>
        {
            var result = await sender.Send(new LogoutCommand { DeviceId = deviceId });
            return result.Success ? Results.Ok() : result.ToProblemDetails();
        })
        .Produces(StatusCodes.Status200OK)
        .Accepts<Guid>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
