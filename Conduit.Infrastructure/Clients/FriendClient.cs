using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Friends.Queries.GetFriendRequests;
using Conduit.Domain.Common;
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
        var response = await _client.GetAsync("friendships", ct);
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
        var response = await _client.PostAsJsonAsync("friendship", new { FriendTag = friendTag }, ct);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task<IReadOnlyCollection<GetFriendRequestsDto>> GetFriendRequestsAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("friendship/requests", ct);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("FriendClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return [];
        }
        var envelope = await response.Content.ReadFromJsonAsync<Envelope<GetFriendRequestsDto[]>>(cancellationToken: ct);
        return envelope?.Result ?? [];
    }

    public async Task<Result> DeleteFriendshipAsync(string friendTag, CancellationToken ct = default)
    {
        var encodedTag = Uri.EscapeDataString(friendTag);
        var response = await _client.DeleteAsync($"friendship/delete?friendtag={encodedTag}", ct);
        
        if (response.IsSuccessStatusCode) return Result.Ok(); 
        
        var envelope = await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct);
        return Result.Fail(Error.Failure("Internal.Server.Error", envelope?.ErrorMessage ?? "An error occurred while deleting the friendship."));
    }
}