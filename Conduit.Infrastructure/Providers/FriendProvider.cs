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
            return await response.ToFailureResultAsync<IReadOnlyCollection<Friend>>(ct, fromDetails: true);
        }

        if (response.StatusCode == HttpStatusCode.NoContent) return Result.Ok<IReadOnlyCollection<Friend>>([]);
        var value = await response.Content.ReadFromJsonAsync<Friend[]>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Response content is null");
        return Result.Ok<IReadOnlyCollection<Friend>>(value);
    }

    public async Task<Result> CreateFriendshipAsync(string friendTag, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("friendship", new { FriendTag = friendTag }, ct);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("FriendClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync(ct, fromDetails: true);
        }
        return Result.Ok();
    }

    public async Task<Result<IReadOnlyCollection<GetFriendRequestsDto>>> GetFriendRequestsAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("friendship/requests", ct);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("FriendClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync<IReadOnlyCollection<GetFriendRequestsDto>>(ct, fromDetails: true);
        }
        if (response.StatusCode == HttpStatusCode.NoContent) return Result.Ok<IReadOnlyCollection<GetFriendRequestsDto>>([]);
        var value = await response.Content.ReadFromJsonAsync<GetFriendRequestsDto[]>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Response content is null");
        return Result.Ok<IReadOnlyCollection<GetFriendRequestsDto>>(value);
    }

    public async Task<Result> DeleteFriendshipAsync(string friendTag, CancellationToken ct = default)
    {
        var encodedTag = Uri.EscapeDataString(friendTag);
        var response = await _client.DeleteAsync($"friendship/delete?friendtag={encodedTag}", ct);

        return response.IsSuccessStatusCode ? Result.Ok() : await response.ToFailureResultAsync(ct, fromDetails: true);
    }

    public async Task<Result> BlockUserAsync(string userTag, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("friendship/block-user", new { BlockedUserTag = userTag }, ct);
        return response.IsSuccessStatusCode ? Result.Ok() : await response.ToFailureResultAsync(ct, fromDetails: true);
    }
}
