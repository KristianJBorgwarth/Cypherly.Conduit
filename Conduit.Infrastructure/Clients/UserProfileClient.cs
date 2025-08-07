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
    
    public async Task<UserProfile?> GetUserProfile()
    {
        var response = await _client.GetAsync("");

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get user profile: {ResponseReasonPhrase}", response.ReasonPhrase);
            return null;
        }

        var envelope = await response.Content.ReadFromJsonAsync<Envelope<UserProfile>>();

        return envelope?.Result;
    }
}