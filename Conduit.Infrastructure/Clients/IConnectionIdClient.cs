using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Clients;

internal sealed class ConnectionIdClient(
    IHttpClientFactory clientFactory,
    ILogger<ConnectionIdClient> logger)
    : IConnectionIdProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.ConnectionIdClient);

    public async Task<List<Guid>> GetConnectionIds(CancellationToken cancellationToken = default)
    {
        foreach (var header in _client.DefaultRequestHeaders)
        {
            logger.LogInformation("Outgoing Header: {Key} = {Value}", header.Key, string.Join(", ", header.Value));
        }
        
        var response = await _client.GetAsync("", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ConnectionIdClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return [];
        }

        var envelope = await response.Content.ReadFromJsonAsync<Envelope<List<Guid>>>(cancellationToken: cancellationToken);
        
        return envelope?.Result ?? [];
    }
}