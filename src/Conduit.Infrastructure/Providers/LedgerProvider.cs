using System.Net.Http.Json;
using Conduit.Application.Contracts.Providers;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Constants;
using Conduit.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Conduit.Infrastructure.Providers;

internal sealed class LedgerProvider(
    IHttpClientFactory clientFactory,
    ILogger<LedgerProvider> logger)
    : ILedgerProvider
{
    private readonly HttpClient _client = clientFactory.CreateClient(ClientNames.LedgerClient);

    public async Task<Result> CreateLedgerAsync(
        Guid ledgerId,
        byte[] payload,
        byte[] writeKey,
        byte[] signature,
        CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync(
            "streams",
            new
            {
                LedgerId = ledgerId,
                Payload = payload,
                WriteKey = writeKey,
                Signature = signature
            },
            ct);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("LedgerClient failed with status code {ResponseStatusCode}", response.StatusCode);
            return await response.ToFailureResultAsync(ct, fromDetails: true);
        }

        return Result.Ok();
    }
}
