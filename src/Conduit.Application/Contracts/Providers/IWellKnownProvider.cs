using Conduit.Application.Features.WellKnown.Dtos;
using Conduit.Domain.Common;

namespace Conduit.Application.Contracts.Providers;

public interface IWellKnownProvider
{
    public Task<Result<TokenIssuerDirectoryDto>> GetTokenIssuerDirectoryAsync(CancellationToken ct = default);
}
