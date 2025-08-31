using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;
using Conduit.Domain.Models;
using Conduit.Infrastructure.Constants;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Clients;

internal sealed class UserProfileClient(
    IHttpClientFactory factory,
    ILogger<UserProfileClient> logger) 
    : IUserProfileProvider
{
    private readonly HttpClient _client = factory.CreateClient(ClientNames.UserProfileClient);
    
    public async Task<UserProfile?> GetUserProfile(Guid exclusiveConnectionId, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync($"?ExclusiveConnectionId={exclusiveConnectionId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get user profile: {ResponseReasonPhrase}", response.ReasonPhrase);
            return null;
        }

        var envelope = await response.Content.ReadFromJsonAsync<Envelope<UserProfile>>(cancellationToken: cancellationToken);

        return envelope?.Result;
    }
    
    public async Task<GetUserProfileByTagDto?> GetUserProfileByTag(string userTag, CancellationToken cancellationToken = default)
    {
        var encodedTag = Uri.EscapeDataString(userTag);
        var response = await _client.GetAsync($"tag?tag={encodedTag}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get user profile by tag: {ResponseReasonPhrase}", response.ReasonPhrase);
            return null;
        }

        var envelope = await response.Content.ReadFromJsonAsync<Envelope<GetUserProfileByTagDto>>(cancellationToken: cancellationToken);

        return envelope?.Result;
    }
}