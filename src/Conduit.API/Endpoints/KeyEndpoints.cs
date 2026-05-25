using Conduit.API.Common;
using Conduit.API.Requests;
using Conduit.Application.Features.Keys.Commands.UploadOneTimePreKeys;
using Conduit.Application.Features.Keys.Dtos;
using Conduit.Application.Features.Keys.Queries;
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

        group.MapGet("/session", async ([FromQuery] Guid accessKey, ISender sender) =>
            {
                var result = await sender.Send(new GetSessionKeysQuery { AccessKey = accessKey });

                return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
            })
            .Produces<SessionKeysDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
