using System.Net;
using System.Net.Http.Json;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Clients;

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace Conduit.Infrastructure.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<Result<T>> ToFailureResultAsync<T>(
        this HttpResponseMessage response,
        CancellationToken ct = default)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => await HandleBadRequest<T>(response, ct),
            HttpStatusCode.NotFound   => await HandleNotFound<T>(response, ct),
            HttpStatusCode.Unauthorized => Result.Fail<T>(Error.Unauthorized("Unauthorized", "Request requires user authentication.")),
            HttpStatusCode.InternalServerError => Result.Fail<T>(Error.Failure("Internal.Server.Error", "An internal server error occurred calling downstream service.")),
            _ => Result.Fail<T>(Error.Failure("Internal.Server.Error", "An unknown error occured calling downstream service."))
        };
    }

    public static async Task<Result> ToFailureResultAsync(this HttpResponseMessage response,
        CancellationToken ct = default)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => await HandleBadRequest(response, ct),
            HttpStatusCode.NotFound => await HandleNotFound(response, ct),
            HttpStatusCode.Unauthorized => Result.Fail(Error.Unauthorized("Unauthorized", "Request requires user authentication.")),
            HttpStatusCode.InternalServerError => Result.Fail(Error.Failure("Internal.Server.Error", "An internal server error occurred calling downstream service.")),
            _ => Result.Fail(Error.Failure("Internal.Server.Error", "An unknown error occured calling downstream service."))
        };
    }

    public static async Task<T> GetValueFromEnvelopeAsync<T>(this HttpResponseMessage response, CancellationToken ct = default)
    {
        var envelope = await response.Content.ReadFromJsonAsync<Envelope<T>>(cancellationToken: ct);
        return envelope is not null ? envelope.Result : throw new InvalidOperationException("Response content cannot be null for a successful result.");
    }
    
    private static async Task<Result<T>> HandleBadRequest<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var envelope = await response.Content.ReadFromJsonAsync<Envelope<T>>(cancellationToken: ct);

        if (!string.IsNullOrWhiteSpace(envelope?.ErrorMessage) && envelope.ErrorMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Fail<T>(Error.NotFound("Not.Found", envelope.ErrorMessage ?? $"Entity of type {typeof(T).Name} was not found."));
        }

        return Result.Fail<T>(Error.BadRequest("Bad.Request", envelope?.ErrorMessage ?? "Bad request."));
    }

    private static async Task<Result> HandleBadRequest(HttpResponseMessage response, CancellationToken ct)
    {
        var envelope = await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct);

        if (!string.IsNullOrWhiteSpace(envelope?.ErrorMessage) && envelope.ErrorMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Fail(Error.NotFound("Not.Found", envelope.ErrorMessage ?? "Entity was not found."));
        }

        return Result.Fail(Error.BadRequest("Bad.Request", envelope?.ErrorMessage ?? "Bad request."));
    }

    private static async Task<Result<T>> HandleNotFound<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var envelope = await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct);
        return Result.Fail<T>(Error.NotFound("Not.Found", envelope?.ErrorMessage ?? $"Entity of type {typeof(T).Name} was not found."));
    }

    private static async Task<Result> HandleNotFound(HttpResponseMessage response, CancellationToken ct)
    {
        var envelope = await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct);
        return Result.Fail(Error.NotFound("Not.Found", envelope?.ErrorMessage ?? "Entity was not found."));
    }
}