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

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get user profile: {ResponseReasonPhrase}", response.ReasonPhrase);
            return await response.ToFailureResultAsync<UserProfile>(ct: cancellationToken);
        }
        
        return await response.GetValueFromEnvelopeAsync<UserProfile>(ct: cancellationToken);
    }
    
    public async Task<Result<GetUserProfileByTagDto>> GetUserProfileByTag(string userTag, CancellationToken cancellationToken = default)
    {
        var encodedTag = Uri.EscapeDataString(userTag);
        var response = await _client.GetAsync($"tag?tag={encodedTag}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get user profile by tag: {ResponseReasonPhrase}", response.ReasonPhrase); 
            return await response.ToFailureResultAsync<GetUserProfileByTagDto>(ct: cancellationToken);
        }

        if (response.StatusCode == HttpStatusCode.NoContent) return Result.Ok<GetUserProfileByTagDto>();
        return await response.GetValueFromEnvelopeAsync<GetUserProfileByTagDto>(ct: cancellationToken);
    }
}