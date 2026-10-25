using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.WellKnown.Dtos;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class WellKnownProvider(
    IHttpClientFactory clientFactory,
    ILogger<WellKnownProvider> logger)
    : IWellKnownProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.IdentityWellKnownClient);

    public async Task<Result<TokenIssuerDirectoryDto>> GetTokenIssuerDirectoryAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("private-token-issuer-directory", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("IdentityWellKnownClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<TokenIssuerDirectoryDto>(ct);
        }

        return await response.GetValueAsync<TokenIssuerDirectoryDto>(ct);
    }
}
