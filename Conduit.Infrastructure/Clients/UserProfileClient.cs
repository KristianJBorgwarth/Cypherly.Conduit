using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
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
        foreach (var header in _client.DefaultRequestHeaders)
        {
            logger.LogInformation("Outgoing Header: {Key} = {Value}", header.Key, string.Join(", ", header.Value));
        }
        
        var response = await _client.GetAsync($"?ExclusiveConnectionId={exclusiveConnectionId}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get user profile: {ResponseReasonPhrase}", response.ReasonPhrase);
            return null;
        }

        var envelope = await response.Content.ReadFromJsonAsync<Envelope<UserProfile>>(cancellationToken: cancellationToken);

        return envelope?.Result;
    }
}