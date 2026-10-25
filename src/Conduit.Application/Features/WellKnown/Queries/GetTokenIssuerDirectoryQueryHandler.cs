using Conduit.Application.Abstractions;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.WellKnown.Dtos;
using Conduit.Domain.Common;

namespace Conduit.Application.Features.WellKnown.Queries;

public sealed class GetTokenIssuerDirectoryQueryHandler(IWellKnownProvider wellKnownProvider)
    : IQueryHandler<GetTokenIssuerDirectoryQuery, TokenIssuerDirectoryDto>
{
    private const string IssuerRequestUri = "/api/identity/verify-nonce";

    public async ValueTask<Result<TokenIssuerDirectoryDto>> Handle(GetTokenIssuerDirectoryQuery request, CancellationToken ct)
    {
        var result = await wellKnownProvider.GetTokenIssuerDirectoryAsync(ct);
        if (!result.Success) return result;

        return result.RequiredValue with { IssuerRequestUri = IssuerRequestUri };
    }
}
