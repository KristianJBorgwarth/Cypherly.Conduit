using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.UserProfile.Commands.UpdateProfilePicture;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class UserProfileSettingsProvider(
    IHttpClientFactory factory,
    ILogger<UserProfileSettingsProvider> logger)
    : IUserProfileSettingsProvider
{
    private readonly HttpClient _client = factory.CreateClient(ClientNames.UserProfileClient);

    public async Task<Result<UpdateProfilePictureDto>> UpdateProfilePicture(IFormFile newProfilePicture, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        await using var stream = newProfilePicture.OpenReadStream();
        var fileContent = new StreamContent(stream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(newProfilePicture.ContentType);

        content.Add(fileContent, "ProfilePicture", newProfilePicture.FileName);

        var response = await _client.PostAsync("profile-picture", content, ct);
        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to update profile picture: {ResponseReasonPhrase}", response.ReasonPhrase);
            return await response.ToFailureResultAsync<UpdateProfilePictureDto>(ct, fromDetails: true);
        }
        var value = await response.Content.ReadFromJsonAsync<UpdateProfilePictureDto>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Response content is null");
        return Result.Ok(value);
    }

    public async Task<Result<Avatar>> GetAvatarAsync(Guid FileKey, CancellationToken ct = default)
    {
        var response = await _client.GetAsync($"avatar?FileKey={FileKey}", ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to get avatar: {ResponseReasonPhrase}", response.ReasonPhrase);
            return await response.ToFailureResultAsync<Avatar>(ct, fromDetails: true);
        }
        else if (response.StatusCode == HttpStatusCode.NotModified)
        {
            return Result.Ok(new Avatar { Etag = response.GetEtag() });
        }

        var stream = await response.Content.ReadAsStreamAsync(ct);

        var avatar = new Avatar
        {
            Content = stream,
            ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream",
            Etag = response.GetEtag()
        };

        return Result.Ok(avatar);
    }

    public async Task<Result> ToggleProfilePrivacyAsync(bool isPrivate, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("toggle-privacy", new { IsPrivate = isPrivate }, ct);
        return response.IsSuccessStatusCode ? Result.Ok() : await response.ToFailureResultAsync(ct, fromDetails: true);
    }
}
