using Conduit.API.Common;
using Conduit.API.RateLimiting;
using Conduit.Domain.Common;

namespace Conduit.API.Extensions;

internal static class RateLimitingExtensions
{
    public static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = (ctx, _) =>
                new ValueTask(Result.Fail(Error.TooManyRequests()).ToProblemDetails().ExecuteAsync(ctx.HttpContext));

            options.AddPolicy<string, PreKeyFetchPolicy>(PreKeyFetchPolicy.Name);
        });
    }
}
