using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Conduit.Domain.Common;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace Conduit.Infrastructure.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<Result<T>> ToFailureResultAsync<T>(
        this HttpResponseMessage response,
        CancellationToken ct = default,
        bool fromDetails = false)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => fromDetails ? await HandleBadRequestProblemDetails<T>(response, ct) : await HandleBadRequest<T>(response, ct),
            HttpStatusCode.NotFound   => fromDetails ? await HandleNotFoundProblemDetails<T>(response, ct) : await HandleNotFound<T>(response, ct),
            HttpStatusCode.Unauthorized => Result.Fail<T>(Error.Unauthorized("Request requires user authentication.")),
            HttpStatusCode.InternalServerError => Result.Fail<T>(Error.Failure("An internal server error occurred calling downstream service.")),
            _ => Result.Fail<T>(Error.Failure("An unknown error occured calling downstream service."))
        };
    }

    public static async Task<Result> ToFailureResultAsync(this HttpResponseMessage response, CancellationToken ct = default, bool fromDetails = false)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => fromDetails ? await HandleBadRequestProblemDetails(response, ct) : await HandleBadRequest(response, ct),
            HttpStatusCode.NotFound => fromDetails ? await HandleNotFoundProblemDetails(response, ct) : await HandleNotFound(response, ct) ,
            HttpStatusCode.Unauthorized => Result.Fail(Error.Unauthorized("Request requires user authentication.")),
            HttpStatusCode.InternalServerError => Result.Fail(Error.Failure("An internal server error occurred calling downstream service.")),
            _ => Result.Fail(Error.Failure("An unknown error occured calling downstream service."))
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
            return Result.Fail<T>(Error.NotFound(envelope.ErrorMessage ?? $"Entity of type {typeof(T).Name} was not found."));
        }

        return Result.Fail<T>(Error.BadRequest("Bad.Request", envelope?.ErrorMessage ?? "Bad request."));
    }

    private static async Task<Result> HandleBadRequest(HttpResponseMessage response, CancellationToken ct)
    {
        var envelope = await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct);

        if (!string.IsNullOrWhiteSpace(envelope?.ErrorMessage) && envelope.ErrorMessage.Contains("not found", StringComparison.OrdinalIgnoreCase))
        {
            return Result.Fail(Error.NotFound(envelope.ErrorMessage));
        }

        return Result.Fail(Error.BadRequest("Bad.Request", envelope?.ErrorMessage ?? "Bad request."));
    }

    private static async Task<Result<T>> HandleNotFound<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var envelope = await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct);
        return Result.Fail<T>(Error.NotFound(envelope?.ErrorMessage ?? $"Entity of type {typeof(T).Name} was not found."));
    }

    private static async Task<Result> HandleNotFound(HttpResponseMessage response, CancellationToken ct)
    {
        var envelope = await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct);
        return Result.Fail(Error.NotFound(envelope?.ErrorMessage));
    }
    
    public static async Task<Result<T>> HandleNotFoundProblemDetails<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync<ErrorProblemDetails>(cancellationToken: ct);

        if (problemDetails is null) return Result.Fail<T>(Error.NotFound($"Entity of type {typeof(T).Name} was not found."));

        var error = problemDetails.Errors?.FirstOrDefault() ?? Error.NotFound($"Entity of type {typeof(T).Name} was not found.");

        return Result.Fail<T>(error);
    }
    
    public static async Task<Result<T>> HandleBadRequestProblemDetails<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync<ErrorProblemDetails>(cancellationToken: ct);

        if (problemDetails is null) return Result.Fail<T>(Error.BadRequest("Bad.Request", "Bad request."));
        
        var error = problemDetails.Errors?.FirstOrDefault() ?? new Error("bad.request", ErrorType.BadRequest, problemDetails.Detail ?? "Bad request.");

        return Result.Fail<T>(error);
    }
    
    private static async Task<Result> HandleBadRequestProblemDetails(HttpResponseMessage response, CancellationToken ct)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync<ErrorProblemDetails>(cancellationToken: ct);

        if (problemDetails is null) return Result.Fail(Error.BadRequest("Bad.Request", "Bad request."));
        
        var error = problemDetails.Errors?.FirstOrDefault() ?? new Error("bad.request", ErrorType.BadRequest, problemDetails.Detail ?? "Bad request.");

        return Result.Fail(error);
    }
    
    private static async Task<Result> HandleNotFoundProblemDetails(HttpResponseMessage response, CancellationToken ct)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync<ErrorProblemDetails>(cancellationToken: ct);

        if (problemDetails is null) return Result.Fail(Error.NotFound());

        var error = problemDetails.Errors?.FirstOrDefault() ?? Error.NotFound(problemDetails.Detail);

        return Result.Fail(error);
    }
    
    internal class Envelope<T>
    {
        public T Result { get; init; } = default!;
        public string? ErrorMessage { get; init; }
        public required DateTime TimeGenerated { get; init; }
    }

    internal class Envelope
    {
        public string? ErrorMessage { get; init; }
        public required DateTime TimeGenerated { get; init; }
    }

    internal class ErrorProblemDetails : ProblemDetails
    {
        public List<Error>? Errors { get; init; }
    }
}