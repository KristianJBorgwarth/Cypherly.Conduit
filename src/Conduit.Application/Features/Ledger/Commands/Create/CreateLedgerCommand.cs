using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Ledger.Commands.Create;

public sealed record CreateLedgerCommand : ICommand
{
    public required Guid LedgerId { get; init; }
    public required byte[] Payload { get; init; }
    public required byte[] WriteKey { get; init; }
    public required byte[] Signature { get; init; }
}
