using Conduit.API.Common;
using Conduit.API.Requests;
using Conduit.Application.Features.Keys.Commands.UploadOneTimePreKeys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Endpoints;

internal sealed class KeyEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/keys")
            .WithTags("keys")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        group.MapPost("/", async ([FromBody] CreateKeyBundleRequest req, ISender sender) =>
            {
                var cmd = req.MapToCommand();
                var result = await sender.Send(cmd);

                return result.Success ? Results.Created() : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPost("/otps", async ([FromBody] UploadOneTimePreKeysRequest req, ISender sender) =>
            {
                var result = await sender.Send(new UploadOneTimePreKeysCommand { PreKeys = req.PreKeys });
                return result.Success ? Results.Created() : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
