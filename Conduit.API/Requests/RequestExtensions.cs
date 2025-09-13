using Conduit.Application.Features.Keys.Commands;

namespace Conduit.API.Requests;

internal static class RequestExtensions
{
    public static CreateKeyBundleCommand MapToCommand(this CreateKeyBundleRequest req) =>
        new()
        {
            AccessKey = req.AccessKey,
            IdentityKey = req.IdentityKey,
            RegistrationId = req.RegistrationId,
            SignedPrekeyId = req.SignedPrekeyId,
            SignedPreKeyPublic = req.SignedPreKeyPublic,
            SignedPreKeySignature = req.SignedPreKeySignature,
            SignedPreKeyTimestamp = req.SignedPreKeyTimestamp
        };
}