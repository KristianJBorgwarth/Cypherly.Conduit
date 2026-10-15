using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Conduit.Domain.Common;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace Conduit.Infrastructure.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<Result<T>> ToFailureResultAsync<T>(this HttpResponseMessage response, CancellationToken ct = default)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => await HandleBadRequestProblemDetails<T>(response, ct),
            HttpStatusCode.NotFound => await HandleNotFoundProblemDetails<T>(response, ct),
            HttpStatusCode.Unauthorized => Result.Fail<T>(Error.Unauthorized("Request requires user authentication.")),
            HttpStatusCode.Forbidden => await HandleProblemDetails<T>(response, ct, Error.Forbidden()),
            HttpStatusCode.Conflict => await HandleProblemDetails<T>(response, ct, Error.Conflict()),
            HttpStatusCode.TooManyRequests => await HandleProblemDetails<T>(response, ct, Error.TooManyRequests()),
            HttpStatusCode.InternalServerError => Result.Fail<T>(Error.Failure("An internal server error occurred calling downstream service.")),
            _ => Result.Fail<T>(Error.Failure("An unknown error occured calling downstream service."))
        };
    }

    public static async Task<Result> ToFailureResultAsync(this HttpResponseMessage response, CancellationToken ct = default)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => await HandleBadRequestProblemDetails(response, ct),
            HttpStatusCode.NotFound => await HandleNotFoundProblemDetails(response, ct),
            HttpStatusCode.Unauthorized => Result.Fail(Error.Unauthorized("Request requires user authentication.")),
            HttpStatusCode.Forbidden => await HandleProblemDetails(response, ct, Error.Forbidden()),
            HttpStatusCode.Conflict => await HandleProblemDetails(response, ct, Error.Conflict()),
            HttpStatusCode.TooManyRequests => await HandleProblemDetails(response, ct, Error.TooManyRequests()),
            HttpStatusCode.InternalServerError => Result.Fail(Error.Failure("An internal server error occurred calling downstream service.")),
            _ => Result.Fail(Error.Failure("An unknown error occured calling downstream service."))
        };
    }

    public static async Task<Result<T>> ToPassthroughFailureResultAsync<T>(this HttpResponseMessage response, CancellationToken ct = default)
    {
        return Result.Fail<T>(await ReadDownstreamErrorAsync(response, ct));
    }

    public static async Task<Result> ToPassthroughFailureResultAsync(this HttpResponseMessage response, CancellationToken ct = default)
    {
        return Result.Fail(await ReadDownstreamErrorAsync(response, ct));
    }

    private static async Task<Error> ReadDownstreamErrorAsync(HttpResponseMessage response, CancellationToken ct)
    {
        ErrorProblemDetails? problemDetails = null;

        try
        {
            problemDetails = await response.Content.ReadFromJsonAsync<ErrorProblemDetails>(cancellationToken: ct);
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
        }

        return problemDetails?.Errors?.FirstOrDefault()
               ?? new Error("downstream.error", ToErrorType(response.StatusCode), problemDetails?.Detail ?? problemDetails?.Title ?? "An error occurred.");
    }

    private static ErrorType ToErrorType(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.BadRequest => ErrorType.BadRequest,
        HttpStatusCode.NotFound => ErrorType.NotFound,
        HttpStatusCode.Unauthorized => ErrorType.Unauthorized,
        HttpStatusCode.Forbidden => ErrorType.Forbidden,
        HttpStatusCode.Conflict => ErrorType.Conflict,
        HttpStatusCode.TooManyRequests => ErrorType.TooManyRequests,
        _ => ErrorType.Failure
    };

    public static async Task<T> GetValueAsync<T>(this HttpResponseMessage response, CancellationToken ct = default)
    {
        return await response.Content.ReadFromJsonAsync<T>(cancellationToken: ct)
               ?? throw new InvalidOperationException("Response content cannot be null for a successful result.");
    }

    private static async Task<Result<T>> HandleNotFoundProblemDetails<T>(HttpResponseMessage response, CancellationToken ct)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync<ErrorProblemDetails>(cancellationToken: ct);

        if (problemDetails is null) return Result.Fail<T>(Error.NotFound($"Entity of type {typeof(T).Name} was not found."));

        var error = problemDetails.Errors?.FirstOrDefault() ?? Error.NotFound($"Entity of type {typeof(T).Name} was not found.");

        return Result.Fail<T>(error);
    }

    private static async Task<Result<T>> HandleBadRequestProblemDetails<T>(HttpResponseMessage response, CancellationToken ct)
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
    
    private static async Task<Result<T>> HandleProblemDetails<T>(HttpResponseMessage response, CancellationToken ct, Error fallback)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync<ErrorProblemDetails>(cancellationToken: ct);

        return Result.Fail<T>(problemDetails?.Errors?.FirstOrDefault() ?? fallback);
    }

    private static async Task<Result> HandleProblemDetails(HttpResponseMessage response, CancellationToken ct, Error fallback)
    {
        var problemDetails = await response.Content.ReadFromJsonAsync<ErrorProblemDetails>(cancellationToken: ct);

        return Result.Fail(problemDetails?.Errors?.FirstOrDefault() ?? fallback);
    }

    internal class ErrorProblemDetails : ProblemDetails
    {
        public List<Error>? Errors { get; init; }
    }
}
