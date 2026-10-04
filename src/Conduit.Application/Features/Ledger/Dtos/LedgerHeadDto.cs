namespace Conduit.Application.Features.Ledger.Dtos;

public sealed record LedgerHeadDto
{
    public required int Version { get; init; }
    public required byte[] Hash { get; init; }
    public required bool Archived { get; init; }
}
