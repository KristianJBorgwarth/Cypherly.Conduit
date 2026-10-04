using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Ledger.Commands.Archive;

public sealed class ArchiveLedgerCommandHandler(ILedgerProvider ledgerProvider) : ICommandHandler<ArchiveLedgerCommand>
{
    public async Task<Result> Handle(ArchiveLedgerCommand request, CancellationToken cancellationToken)
    {
        return await ledgerProvider.ArchiveLedgerAsync(
            request.LedgerId,
            request.ExpectedVersion,
            request.PreviousHash,
            request.Payload,
            request.WriteKeyPublic,
            request.Signature,
            cancellationToken);
    }
}
