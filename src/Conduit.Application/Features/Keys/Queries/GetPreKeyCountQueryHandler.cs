using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Keys.Dtos;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.Keys.Queries;

public sealed class GetPreKeyCountQueryHandler(IKeyProvider keyProvider) : IQueryHandler<GetPreKeyCountQuery, PreKeyCountDto>
{
    public async ValueTask<Result<PreKeyCountDto>> Handle(GetPreKeyCountQuery request, CancellationToken cancellationToken)
    {
        return await keyProvider.GetPreKeyCountAsync(cancellationToken);
    }
}
