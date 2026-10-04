using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Ledger.Commands.Append;

public sealed class AppendLedgerEventCommandHandler(ILedgerProvider ledgerProvider) : ICommandHandler<AppendLedgerEventCommand>
{
    public async Task<Result> Handle(AppendLedgerEventCommand request, CancellationToken cancellationToken)
    {
        return await ledgerProvider.AppendEventAsync(
            request.LedgerId,
            request.ExpectedVersion,
            request.PreviousHash,
            request.Payload,
            request.WriteKeyPublic,
            request.Signature,
            request.KeysAdded,
            request.KeysRemoved,
            cancellationToken);
    }
}
