using Conduit.API.Common;
using Conduit.API.Requests;
using Conduit.Application.Features.Authentication.Commands.Login;
using Conduit.Application.Features.Authentication.Commands.Logout;
using Conduit.Application.Features.Authentication.Commands.VerifyLogin;
using Conduit.Application.Features.Authentication.Commands.VerifyNonce;
using Conduit.Application.Features.Authentication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Endpoints;

internal sealed class IdentityEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/identity")
            .WithTags("identity");

        group.MapPost("login", async (
                [FromServices] ISender sender,
                [FromBody] LoginRequest req,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new LoginCommand { Email = req.Email, Password = req.Password }, ct);
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<LoginDto>()
        .Accepts<LoginRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("logout", async (
                [FromServices] ISender sender,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new LogoutCommand(), ct);
            return result.Success ? Results.Ok() : result.ToProblemDetails();
        })
        .Produces(StatusCodes.Status200OK)
        .Accepts<Guid>("application/json")
        .RequireAuthorization()
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("verify-login", async (
                [FromServices] ISender sender,
                [FromBody] VerifyLoginRequest req,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new VerifyLoginCommand { UserId = req.UserId, LoginVerificationCode = req.LoginVerificationCode }, ct);
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<VerifyLoginDto>()
        .Accepts<VerifyLoginRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("verify-nonce", async (
                [FromServices] ISender sender,
                [FromBody] VerifyNonceRequest req,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new VerifyNonceCommand
            {
                UserId = req.UserId,
                NonceId = req.NonceId,
                DeviceId = req.DeviceId,
                Nonce = req.Nonce
            }, ct);
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<VerifyNonceDto>()
        .Accepts<VerifyNonceRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("nonce", async (
                 [FromServices] ISender sender,
                 [AsParameters] GetNonceRequest req,
                 CancellationToken ct) =>
         {
             var result = await sender.Send(new GetNonceQuery() { UserId = req.UserId, DeviceId = req.DeviceId }, ct);
             return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
         })
         .Produces<GetNonceDto>()
         .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
