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

    public async Task<IReadOnlyCollection<Guid>> GetConnectionIds(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("/connectionId", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ConnectionIdClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return [];
        }

        var envelope = await response.Content.ReadFromJsonAsync<Envelope<ConnectionIdsDto>>(cancellationToken: ct);

        return envelope?.Result?.ConnectionIds ?? [];
    }

    public async Task<Dictionary<Guid, IReadOnlyCollection<Guid>>> GetConnectionIds(IReadOnlyCollection<Guid> userIds,
        CancellationToken ct = default)
    {
        var response = await _client.GetAsync("/connectionIds", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ConnectionIdClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return new Dictionary<Guid, IReadOnlyCollection<Guid>>();
        }

        var envelope = await response.Content.ReadFromJsonAsync<Envelope<MultipleConnectionIds>>(cancellationToken: ct);
        return envelope?.Result?.ConnectionIds ?? new Dictionary<Guid, IReadOnlyCollection<Guid>>();
    }

    private sealed record ConnectionIdsDto(IReadOnlyCollection<Guid> ConnectionIds);
    private sealed record MultipleConnectionIds(Dictionary<Guid, IReadOnlyCollection<Guid>> ConnectionIds);
}