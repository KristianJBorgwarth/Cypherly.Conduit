using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Ledger.Commands.Create;

public sealed class CreateLedgerCommandHandler(ILedgerProvider ledgerProvider) : ICommandHandler<CreateLedgerCommand>
{
    public async Task<Result> Handle(CreateLedgerCommand request, CancellationToken cancellationToken)
    {
        return await ledgerProvider.CreateLedgerAsync(
            request.LedgerId,
            request.Payload,
            request.WriteKey,
            request.Signature,
            cancellationToken);
    }
}
