using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Keys.Commands.UploadOneTimePreKeys;

public sealed class UploadOneTimePreKeysCommandHandler(
    IKeyProvider keyProvider,
    ILogger<UploadOneTimePreKeysCommandHandler> logger)
    : ICommandHandler<UploadOneTimePreKeysCommand>
{
    public async Task<Result> Handle(UploadOneTimePreKeysCommand request, CancellationToken cancellationToken)
    {
        try
        {
            return await keyProvider.UploadOneTimePreKeysAsync(request.PreKeys, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogCritical("An error occurred while uploading one-time pre-keys: {Message}", e.Message);
            return Result.Fail(Error.Failure());
        }
    }
}