using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Authentication.Queries;

public sealed class GetNonceQueryHandler(IIdentityProvider idProvider) : IQueryHandler<GetNonceQuery, GetNonceDto>
{
    public async Task<Result<GetNonceDto>> Handle(GetNonceQuery q, CancellationToken ct)
        => await idProvider.GetNonceAsync(q.UserId, q.DeviceId, ct);
}
