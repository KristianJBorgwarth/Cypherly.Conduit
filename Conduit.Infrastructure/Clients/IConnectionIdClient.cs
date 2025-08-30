using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Infrastructure.Constants;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Clients;

internal sealed class ConnectionIdClient(
    IHttpClientFactory clientFactory,
    ILogger<ConnectionIdClient> logger)
    : IConnectionIdProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.ConnectionIdClient);

    public async Task<IReadOnlyCollection<Guid>> GetConnectionIds(CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync("", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ConnectionIdClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return [];
        }

        var envelope =
            await response.Content.ReadFromJsonAsync<Envelope<ConnectionIdsDto>>(cancellationToken: cancellationToken);

        return envelope?.Result?.ConnectionIds ?? [];
    }
    
    private sealed record ConnectionIdsDto(IReadOnlyCollection<Guid> ConnectionIds);
}
