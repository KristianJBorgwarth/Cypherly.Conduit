// ReSharper disable InvertIf

using System.Net;
using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Friends.Queries.GetFriendRequests;
using Conduit.Domain.Common;
using Conduit.Domain.Models;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class FriendProvider(
    IHttpClientFactory clientFactory,
    ILogger<FriendProvider> logger)
    : IFriendProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.UserProfileClient);

    public async Task<Result<IReadOnlyCollection<Friend>>> GetFriendsAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("friendships", ct);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("FriendClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<IReadOnlyCollection<Friend>>(ct);
        }

        if (response.StatusCode == HttpStatusCode.NoContent) return Result.Ok<IReadOnlyCollection<Friend>>([]);
        return await response.GetValueFromEnvelopeAsync<Friend[]>(ct);
    }

    public async Task<Result> CreateFriendshipAsync(string friendTag, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("friendship", new { FriendTag = friendTag }, ct);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("FriendClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync(ct);
        }
        return Result.Ok();
    }

    public async Task<Result<IReadOnlyCollection<GetFriendRequestsDto>>> GetFriendRequestsAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("friendship/requests", ct);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("FriendClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<IReadOnlyCollection<GetFriendRequestsDto>>(ct);
        }
        if (response.StatusCode == HttpStatusCode.NoContent) return Result.Ok<IReadOnlyCollection<GetFriendRequestsDto>>([]);
        return await response.GetValueFromEnvelopeAsync<GetFriendRequestsDto[]>(ct);
    }

    public async Task<Result> DeleteFriendshipAsync(string friendTag, CancellationToken ct = default)
    {
        var encodedTag = Uri.EscapeDataString(friendTag);
        var response = await _client.DeleteAsync($"friendship/delete?friendtag={encodedTag}", ct);

        return response.IsSuccessStatusCode ? Result.Ok() : await response.ToFailureResultAsync(ct);
    }

    public async Task<Result> BlockUserAsync(string userTag, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("friendship/block-user", new { BlockedUserTag = userTag }, ct);
        return response.IsSuccessStatusCode ? Result.Ok() : await response.ToFailureResultAsync(ct);
    }
}
