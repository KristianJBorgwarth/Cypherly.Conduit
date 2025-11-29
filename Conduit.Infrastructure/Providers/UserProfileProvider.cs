using System.Net;
using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;
using Conduit.Domain.Common;
using Conduit.Domain.Models;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

// ReSharper disable InvertIf

namespace Conduit.Infrastructure.Providers;

internal sealed class UserProfileProvider(
    IHttpClientFactory factory,
    ILogger<UserProfileProvider> logger)
    : IUserProfileProvider
{
    private readonly HttpClient _client = factory.CreateClient(ClientNames.UserProfileClient);

    public async Task<Result<UserProfile>> GetUserProfile(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get user profile: {ResponseReasonPhrase}", response.ReasonPhrase);
            return await response.ToFailureResultAsync<UserProfile>(ct: ct);
        }

        return await response.GetValueFromEnvelopeAsync<UserProfile>(ct: ct);
    }

    public async Task<Result<GetUserProfileByTagDto>> GetUserProfileByTag(string userTag, CancellationToken ct = default)
    {
        var encodedTag = Uri.EscapeDataString(userTag);
        var response = await _client.GetAsync($"tag?tag={encodedTag}", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get user profile by tag: {ResponseReasonPhrase}", response.ReasonPhrase);
            return await response.ToFailureResultAsync<GetUserProfileByTagDto>(ct: ct);
        }

        if (response.StatusCode == HttpStatusCode.NoContent) return Result.Ok<GetUserProfileByTagDto>();
        return await response.GetValueFromEnvelopeAsync<GetUserProfileByTagDto>(ct: ct);
    }

    public async Task<Result<string>> UpdateDisplayName(string newDisplayName, CancellationToken ct = default)
    {
        var response = await _client.PutAsJsonAsync("display-name", new { NewDisplayName = newDisplayName }, ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to update display name: {ResponseReasonPhrase}", response.ReasonPhrase);
            return await response.ToFailureResultAsync<string>(ct: ct);
        }

        return await response.GetValueFromEnvelopeAsync<string>(ct: ct);
    }
}
