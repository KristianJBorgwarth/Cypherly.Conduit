namespace Conduit.API.Requests;

public sealed class CreateKeyBundleRequest
{
    public required Guid AccessKey { get; init; }
    public required byte[] IdentityKey { get; init; }
    public required ushort RegistrationId { get; init; }
    public required int SignedPrekeyId { get; init; }
    public required byte[] SignedPreKeyPublic { get; init; }
    public required byte[] SignedPreKeySignature { get; init; }
    public required DateTimeOffset SignedPreKeyTimestamp { get; init; }
}

