using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;
using Conduit.Application.Features.UserProfile.Queries.GetUserProfileByTag;
using Conduit.Domain.Common;
using Conduit.Domain.Models;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
// ReSharper disable InvertIf

namespace Conduit.Infrastructure.Clients;

internal sealed class UserProfileClient(
    IHttpClientFactory factory,
    ILogger<UserProfileClient> logger) 
    : IUserProfileProvider
{
    private readonly HttpClient _client = factory.CreateClient(ClientNames.UserProfileClient);
    
    public async Task<Result<UserProfile>> GetUserProfile(Guid exclusiveConnectionId, CancellationToken ct = default)
    {
        var response = await _client.GetAsync($"?ExclusiveConnectionId={exclusiveConnectionId}", ct);

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

    public async Task<Result<UpdateProfilePictureDto>> UpdateProfilePicture(IFormFile newProfilePicture, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        await using var stream = newProfilePicture.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(newProfilePicture.ContentType);

        content.Add(fileContent, "ProfilePicture", newProfilePicture.FileName);

        var response = await _client.PostAsync("profile-picture", content, ct);
        if(!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to update profile picture: {ResponseReasonPhrase}", response.ReasonPhrase);
            return await response.ToFailureResultAsync<UpdateProfilePictureDto>(ct: ct);
        }
        return await response.GetValueFromEnvelopeAsync<UpdateProfilePictureDto>(ct);
    }
}