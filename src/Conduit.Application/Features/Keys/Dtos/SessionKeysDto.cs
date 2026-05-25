namespace Conduit.Application.Features.Keys.Dtos;

public sealed record SessionKeysDto
{
    public required byte[] IdentityKey { get; init; }
    public required ushort RegistrationId { get; init; }
    public required int SignedPrekeyId { get; init; }
    public required byte[] SignedPreKeyPublic { get; init; }
    public required byte[] SignedPreKeySignature { get; init; }
    public required PreKeyDto? PreKey { get; init; }
}
