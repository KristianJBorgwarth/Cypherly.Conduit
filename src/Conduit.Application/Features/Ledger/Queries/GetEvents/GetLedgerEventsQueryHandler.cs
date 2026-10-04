using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Ledger.Dtos;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Ledger.Queries.GetEvents;

public sealed class GetLedgerEventsQueryHandler(ILedgerProvider ledgerProvider) : IQueryHandler<GetLedgerEventsQuery, LedgerEventsDto>
{
    public async Task<Result<LedgerEventsDto>> Handle(GetLedgerEventsQuery request, CancellationToken cancellationToken)
    {
        return await ledgerProvider.GetEventsAsync(
            request.LedgerId,
            request.FromVersion,
            request.Limit,
            cancellationToken);
    }
}
