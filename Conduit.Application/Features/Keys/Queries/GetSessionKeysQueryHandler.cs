using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Keys.Dtos;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Keys.Queries;

public sealed class GetSessionKeysQueryHandler(
    IKeyProvider keyProvider,
    ILogger<GetSessionKeysQueryHandler> logger) 
    : IQueryHandler<GetSessionKeysQuery, SessionKeysDto>
{
    public async Task<Result<SessionKeysDto>> Handle(GetSessionKeysQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var sessionKeys = await keyProvider.GetSessionKeysAsync(request.AccessKey, cancellationToken);
            return Result.Ok(sessionKeys);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occured while retrieving session keys for AccessKey {AccessKey}", request.AccessKey);
            return Result.Fail<SessionKeysDto>(Error.Failure("An unexpected error occured"));
        }
    }
}
