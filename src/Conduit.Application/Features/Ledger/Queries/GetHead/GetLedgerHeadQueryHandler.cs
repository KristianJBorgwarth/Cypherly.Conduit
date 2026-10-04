using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Ledger.Dtos;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Ledger.Queries.GetHead;

public sealed class GetLedgerHeadQueryHandler(ILedgerProvider ledgerProvider) : IQueryHandler<GetLedgerHeadQuery, LedgerHeadDto>
{
    public async Task<Result<LedgerHeadDto>> Handle(GetLedgerHeadQuery request, CancellationToken cancellationToken)
    {
        return await ledgerProvider.GetHeadAsync(request.LedgerId, cancellationToken);
    }
}
