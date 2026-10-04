using Conduit.API.Common;
using Conduit.API.Requests;
using Conduit.Application.Features.Ledger.Commands.Append;
using Conduit.Application.Features.Ledger.Commands.Archive;
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

        group.MapPut("/streams/{id:guid}/events", async (
                [FromRoute] Guid id,
                [FromBody] AppendLedgerEventRequest req,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new AppendLedgerEventCommand
                {
                    LedgerId = id,
                    ExpectedVersion = req.ExpectedVersion,
                    PreviousHash = req.PreviousHash,
                    Payload = req.Payload,
                    WriteKeyPublic = req.WriteKeyPublic,
                    Signature = req.Signature,
                    KeysAdded = req.KeysAdded,
                    KeysRemoved = req.KeysRemoved
                }, ct);

                return result.Success ? Results.Ok() : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);

        group.MapPut("/streams/{id:guid}/archive", async (
                [FromRoute] Guid id,
                [FromBody] ArchiveLedgerRequest req,
                ISender sender,
                CancellationToken ct) =>
            {
                var result = await sender.Send(new ArchiveLedgerCommand
                {
                    LedgerId = id,
                    ExpectedVersion = req.ExpectedVersion,
                    PreviousHash = req.PreviousHash,
                    Payload = req.Payload,
                    WriteKeyPublic = req.WriteKeyPublic,
                    Signature = req.Signature
                }, ct);

                return result.Success ? Results.Ok() : result.ToProblemDetails();
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
