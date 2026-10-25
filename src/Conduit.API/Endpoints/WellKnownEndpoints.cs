using Conduit.API.Common;
using Conduit.Application.Features.WellKnown.Dtos;
using Conduit.Application.Features.WellKnown.Queries;
using Mediator;

namespace Conduit.API.Endpoints;

internal sealed class WellKnownEndpoints : IEndpoint
{
    private const string TokenIssuerDirectoryMediaType = "application/private-token-issuer-directory";

    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/.well-known")
            .WithTags("well-known");

        group.MapGet("/private-token-issuer-directory", async (ISender sender, HttpContext http, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetTokenIssuerDirectoryQuery(), ct);
                if (!result.Success) return result.ToProblemDetails();

                http.Response.Headers.CacheControl = "public, max-age=300";

                return Results.Json(result.Value, contentType: TokenIssuerDirectoryMediaType);
            })
            .Produces<TokenIssuerDirectoryDto>(StatusCodes.Status200OK, TokenIssuerDirectoryMediaType);
    }
}
