namespace Conduit.Domain.Models;

public sealed class PreKey
{
    public int KeyId { get; init; }
    public byte[] PublicKey { get; init; } = null!;
}