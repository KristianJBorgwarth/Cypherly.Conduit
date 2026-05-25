using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Keys.Commands.Create;

public sealed class CreateKeyBundleCommandHandler(IKeyProvider keyProvider) : ICommandHandler<CreateKeyBundleCommand>
{
    public async Task<Result> Handle(CreateKeyBundleCommand request, CancellationToken cancellationToken)
    {
        return await keyProvider.CreateKeyBundleAsync(
            request.AccessKey,
            request.IdentityKey,
            request.RegistrationId,
            request.SignedPrekeyId,
            request.SignedPreKeyPublic,
            request.SignedPreKeySignature,
            request.PreKeys,
            request.SignedPreKeyTimestamp,
            cancellationToken);
    }
}
