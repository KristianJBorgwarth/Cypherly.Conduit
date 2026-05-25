using Microsoft.AspNetCore.Http;

public static class HttpExtensions
{
    /// <summary>
    /// Retrieves the ETag value from the HTTP response headers.
    /// </summary> <param name="response">The HTTP response message.</param>
    /// <returns>The ETag value as a string.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the ETag header is missing in the response.</exception>
    public static string GetEtag(this HttpResponseMessage response)
    {
        if (response.Headers.ETag is null)
        {
            throw new InvalidOperationException("ETag header is missing in the response.");
        }

        return response.Headers.ETag.Tag;
    }

    /// <summary>
    /// Adds an ETag header to the HTTP response.
    /// </summary> <param name="response">The HTTP response message.</param> <param name="etag">The ETag value to be added to the response headers.</param>
    public static void AddEtag(this HttpResponse response, string etag)
    {
        response.Headers.ETag = etag;
    }
}
