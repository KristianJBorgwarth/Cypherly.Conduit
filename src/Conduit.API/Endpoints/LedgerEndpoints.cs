using Conduit.API.Common;
using Conduit.API.Requests;
using Conduit.Application.Features.Ledger.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Endpoints;

internal sealed class LedgerEndpoints : IEndpoint
{
    public void MapRoutes(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("api/ledger")
            .WithTags("ledger")
            .RequireAuthorization()
            .ProducesProblem(StatusCodes.Status401Unauthorized);

        group.MapPost("/streams", async (
                [FromBody] CreateLedgerRequest req,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new CreateLedgerCommand
                {
                    LedgerId = req.LedgerId,
                    Payload = req.Payload,
                    WriteKey = req.WriteKey,
                    Signature = req.Signature
                }, ct);

                return result.Success ? Results.Created($"api/ledger/streams/{req.LedgerId}", null) : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
