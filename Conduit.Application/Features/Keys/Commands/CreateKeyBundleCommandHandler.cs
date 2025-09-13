using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Keys.Commands;

public sealed class CreateKeyBundleCommandHandler(
    IKeyProvider keyProvider,
    ILogger<CreateKeyBundleCommandHandler> logger) 
    : ICommandHandler<CreateKeyBundleCommand>
{
    public async Task<Result> Handle(CreateKeyBundleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await keyProvider.CreateKeyBundleAsync(
                request.AccessKey,
                request.IdentityKey,
                request.RegistrationId,
                request.SignedPrekeyId,
                request.SignedPreKeyPublic,
                request.SignedPreKeySignature,
                request.SignedPreKeyTimestamp,
                cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogCritical("An error occurred while creating a key bundle: {Message}", e.Message);
            return Result.Fail(Error.Failure());
        }
    }
}