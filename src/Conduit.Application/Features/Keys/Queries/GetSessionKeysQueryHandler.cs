using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Keys.Dtos;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Keys.Queries;

public sealed class GetSessionKeysQueryHandler(IKeyProvider keyProvider) : IQueryHandler<GetSessionKeysQuery, SessionKeysDto>
{
    public async Task<Result<SessionKeysDto>> Handle(GetSessionKeysQuery request, CancellationToken cancellationToken)
    {
        var sessionKeys = await keyProvider.GetSessionKeysAsync(request.AccessKey, cancellationToken);
        return Result.Ok(sessionKeys);
    }
}
