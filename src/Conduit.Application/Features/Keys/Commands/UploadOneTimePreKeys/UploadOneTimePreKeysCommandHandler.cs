using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Keys.Commands.UploadOneTimePreKeys;

public sealed class UploadOneTimePreKeysCommandHandler(IKeyProvider keyProvider) : ICommandHandler<UploadOneTimePreKeysCommand>
{
    public async Task<Result> Handle(UploadOneTimePreKeysCommand request, CancellationToken cancellationToken)
    {
        return await keyProvider.UploadOneTimePreKeysAsync(request.PreKeys, cancellationToken);
    }
}
