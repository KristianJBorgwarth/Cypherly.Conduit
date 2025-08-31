using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;
using Conduit.Domain.Common;
using Conduit.Domain.Models;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Clients;

internal sealed class UserProfileClient(
    IHttpClientFactory factory,
    ILogger<UserProfileClient> logger) 
    : IUserProfileProvider
{
    private readonly HttpClient _client = factory.CreateClient(ClientNames.UserProfileClient);
    
    public async Task<Result<UserProfile>> GetUserProfile(Guid exclusiveConnectionId, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync($"?ExclusiveConnectionId={exclusiveConnectionId}", cancellationToken);

        if (!response.IsSuccessStatusCode) return await response.ToFailureResultAsync<UserProfile>(ct: cancellationToken);
        
        return await response.GetValueFromEnvelopeAsync<UserProfile>(ct: cancellationToken);
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