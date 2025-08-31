using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Models;
using Conduit.Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Clients;

internal sealed class FriendClient(
    IHttpClientFactory clientFactory,
    ILogger<FriendClient> logger) 
    : IFriendProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.UserProfileClient);
    public async Task<IReadOnlyCollection<Friend>> GetFriendsAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("/friendships", ct);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("FriendClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return [];
        }
        var envelope = await response.Content.ReadFromJsonAsync<Envelope<Friend[]>>(cancellationToken: ct);
        return envelope?.Result ?? [];
    }

    public async Task CreateFriendshipAsync(string friendTag, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("/friendship", new { FriendTag = friendTag }, ct);
        
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("FriendClient failed with status code {ResponseStatusCode}", response.StatusCode);
            throw new Exception("Failed to create friendship");
        }
    }
}