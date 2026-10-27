using Conduit.Application.Contracts.Providers;
using Conduit.Application.Features.Device.Queries.GetDevices;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class ChatServerProvider(
    IHttpClientFactory clientFactory,
    ILogger<ChatServerProvider> logger)
    : IChatServerProvider
{
    private static readonly Error Unreachable = Error.Failure("An unexpected error occurred.");

    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.ChatServerClient);

    public async Task<Result<PresenceDto>> GetPresenceAsync(CancellationToken ct = default)
    {
        var response = await SendAsync(() => _client.GetAsync("presence", ct), ct);

        if (response is null)
            return Result.Fail<PresenceDto>(Unreachable);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("ChatServerClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToPassthroughFailureResultAsync<PresenceDto>(ct);
        }

        return await response.GetValueAsync<PresenceDto>(ct);
    }

    private async Task<HttpResponseMessage?> SendAsync(Func<Task<HttpResponseMessage>> send, CancellationToken ct)
    {
        try
        {
            return await send();
        }
        catch (Exception ex) when (ex is HttpRequestException || (ex is TaskCanceledException && !ct.IsCancellationRequested))
        {
            logger.LogError(ex, "ChatServerClient could not reach ChatServer");
            return null;
        }
    }
}
