using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class ConnectionIdProvider(
    IHttpClientFactory clientFactory,
    ILogger<ConnectionIdProvider> logger)
    : IConnectionIdProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.ConnectionIdClient);

    public async Task<Result<IReadOnlyCollection<Guid>>> GetConnectionIds(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("device/connectionid", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ConnectionIdClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<IReadOnlyCollection<Guid>>(ct, fromDetails: true);
        }

        var value = await response.Content.ReadFromJsonAsync<ConnectionIdsDto>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Response content is null");
        return Result.Ok(value.ConnectionIds);
    }

    public async Task<Result<Dictionary<Guid, IReadOnlyCollection<Guid>>>> GetConnectionIds(IReadOnlyCollection<Guid> userIds,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("devices/connectionids", new
        {
            TenantIds = userIds
        }, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ConnectionIdClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<Dictionary<Guid, IReadOnlyCollection<Guid>>>(ct, fromDetails: true);
        }

        var value = await response.Content.ReadFromJsonAsync<MultipleConnectionIds>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Response content is null");
        return Result.Ok(value.ConnectionIds);
    }

    private sealed record ConnectionIdsDto(IReadOnlyCollection<Guid> ConnectionIds);
    private sealed record MultipleConnectionIds(Dictionary<Guid, IReadOnlyCollection<Guid>> ConnectionIds);
}
