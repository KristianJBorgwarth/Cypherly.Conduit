using System.Net;
using System.Net.Http.Json;
using Conduit.Domain.Common;
using Conduit.Infrastructure.Clients;
// ReSharper disable SwitchStatementHandlesSomeKnownEnumValuesWithDefault

namespace Conduit.Infrastructure.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<Result<T>> ToFailureResultAsync<T>(this HttpResponseMessage response, CancellationToken ct = default)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => 
                Result.Fail<T>(Error.BadRequest(
                    "Bad.Request",
                    (await response.Content.ReadFromJsonAsync<Envelope<T>>(cancellationToken: ct))?.ErrorMessage 
                    ?? "A bad request was made calling downstream service.")),

            HttpStatusCode.NotFound => 
                Result.Fail<T>(Error.NotFound(
                    "Not.Found",
                    (await response.Content.ReadFromJsonAsync<Envelope<T>>(cancellationToken: ct))?.ErrorMessage 
                    ?? $"Entity of type {typeof(T).Name} was not found.")),

            HttpStatusCode.Unauthorized => 
                Result.Fail<T>(Error.Unauthorized(
                    "Unauthorized",
                    "Request requires user authentication.")),

            HttpStatusCode.InternalServerError => 
                Result.Fail<T>(Error.Failure(
                    "Internal.Server.Error",
                    "An internal server error occurred calling downstream service.")),

            _ => 
                Result.Fail<T>(Error.Failure(
                    "Internal.Server.Error",
                    $"An unknown error occurred calling downstream service. ({(int)response.StatusCode} {response.ReasonPhrase})"))
        };
    }

    
    public static async Task<Result> ToFailureResultAsync(this HttpResponseMessage response, CancellationToken ct = default)
    {
        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => 
                Result.Fail(Error.BadRequest(
                    "Bad.Request",
                    (await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct))?.ErrorMessage 
                    ?? "Bad request.")),

            HttpStatusCode.NotFound => 
                Result.Fail(Error.NotFound(
                    "Not.Found",
                    (await response.Content.ReadFromJsonAsync<Envelope>(cancellationToken: ct))?.ErrorMessage 
                    ?? "Entity was not found.")),

            HttpStatusCode.Unauthorized => 
                Result.Fail(Error.Unauthorized(
                    "Unauthorized",
                    "Request requires user authentication.")),

            HttpStatusCode.InternalServerError => 
                Result.Fail(Error.Failure(
                    "Internal.Server.Error",
                    "An internal server error occurred calling downstream service.")),

            _ => 
                Result.Fail(Error.Failure(
                    "Internal.Server.Error",
                    $"An unknown error occurred calling downstream service. ({(int)response.StatusCode} {response.ReasonPhrase})"))
        };
    }

}