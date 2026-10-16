using Conduit.API.Common;
using Conduit.API.Requests;
using Conduit.Application.Features.Account.Commands.Register;
using Conduit.Application.Features.Account.Commands.ResendVerificationCode;
using Conduit.Application.Features.Account.Commands.Verify;
using Conduit.Application.Features.Authentication.Commands.Login;
using Conduit.Application.Features.Authentication.Commands.Logout;
using Conduit.Application.Features.Authentication.Commands.RefreshTokens;
using Conduit.Application.Features.Authentication.Commands.VerifyLogin;
using Conduit.Application.Features.Authentication.Commands.VerifyNonce;
using Conduit.Application.Features.Authentication.Queries;
using Conduit.Application.Features.Device.Commands.Create;
using Mediator;
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
            var result = await sender.Send(new VerifyLoginCommand { UserId = req.UserId, LoginVerificationCode = req.LoginVerificationCode, NonceId = req.NonceId, Nonce = req.Nonce }, ct);
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<VerifyLoginDto>()
        .Accepts<VerifyLoginRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized);

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
                Nonce = req.Nonce,
                Announce = req.Announce
            }, ct);
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<VerifyNonceDto>()
        .Accepts<VerifyNonceRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPost("refresh-token", async (
                [FromServices] ISender sender,
                [FromBody] RefreshTokensRequest req,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new RefreshTokensCommand
            {
                UserId = req.UserId,
                DeviceId = req.DeviceId,
                RefreshToken = req.RefreshToken
            }, ct);
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<RefreshTokensDto>()
        .Accepts<RefreshTokensRequest>("application/json")
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

        var user = group.MapGroup("user");

        user.MapPost("", async (
                [FromServices] ISender sender,
                [FromBody] RegisterRequest req,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new RegisterCommand { Username = req.Username, Email = req.Email, Password = req.Password }, ct);
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<RegisterDto>()
        .Accepts<RegisterRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict);

        user.MapPut("verify", async (
                [FromServices] ISender sender,
                [FromBody] VerifyAccountRequest req,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new VerifyAccountCommand { UserId = req.UserId, VerificationCode = req.VerificationCode }, ct);
            return result.Success ? Results.Ok() : result.ToProblemDetails();
        })
        .Produces(StatusCodes.Status200OK)
        .Accepts<VerifyAccountRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status429TooManyRequests);

        user.MapPut("resend-verification", async (
                [FromServices] ISender sender,
                [FromBody] ResendVerificationCodeRequest req,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new ResendVerificationCodeCommand
            {
                UserId = req.UserId,
                CodeType = req.CodeType,
                NonceId = req.NonceId,
                Nonce = req.Nonce
            }, ct);
            return result.Success ? Results.Ok() : result.ToProblemDetails();
        })
        .Produces(StatusCodes.Status200OK)
        .Accepts<ResendVerificationCodeRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status429TooManyRequests);

        user.MapPost("device", async (
                [FromServices] ISender sender,
                [FromBody] CreateDeviceRequest req,
                CancellationToken ct) =>
        {
            var result = await sender.Send(new CreateDeviceCommand
            {
                UserId = req.UserId,
                LoginNonceId = req.LoginNonceId,
                LoginNonce = req.LoginNonce,
                DeviceAppVersion = req.DeviceAppVersion,
                DeviceType = req.DeviceType,
                DevicePlatform = req.DevicePlatform,
                Base64DevicePublicKey = req.Base64DevicePublicKey
            }, ct);
            return result.Success ? Results.Ok(result.Value) : result.ToProblemDetails();
        })
        .Produces<CreateDeviceDto>()
        .Accepts<CreateDeviceRequest>("application/json")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
