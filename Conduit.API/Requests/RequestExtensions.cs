using Conduit.Application.Features.Keys.Commands;
using Conduit.Application.Features.Keys.Commands.Create;

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
            PreKeys = req.PreKeys,
            SignedPreKeyTimestamp = req.SignedPreKeyTimestamp
        };
}