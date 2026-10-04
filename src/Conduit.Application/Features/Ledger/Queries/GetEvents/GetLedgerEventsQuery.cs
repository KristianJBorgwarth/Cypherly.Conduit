using Conduit.Application.Abstractions;
using Conduit.Application.Features.Ledger.Dtos;

namespace Conduit.Application.Features.Ledger.Queries.GetEvents;

public sealed record GetLedgerEventsQuery : IQuery<LedgerEventsDto>
{
    public required Guid LedgerId { get; init; }
    public required int FromVersion { get; init; }
    public int? Limit { get; init; }
}
