using Conduit.Application.Abstractions;
using Conduit.Application.Features.Ledger.Dtos;

namespace Conduit.Application.Features.Ledger.Queries.GetHead;

public sealed record GetLedgerHeadQuery : IQuery<LedgerHeadDto>
{
    public required Guid LedgerId { get; init; }
}
