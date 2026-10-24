using System.Threading.RateLimiting;
using Conduit.API.Common;
using Conduit.Domain.Common;
using RedisRateLimiting;
using StackExchange.Redis;

namespace Conduit.API.Extensions;

internal static class RateLimitingExtensions
{
    public const string PreKeyFetchPolicy = "prekey-fetch";

    private const int PreKeyFetchPerCaller = 200;
    private const int PreKeyFetchPerTarget = 100;
    private static readonly TimeSpan PreKeyFetchWindow = TimeSpan.FromHours(1);

    public static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = (ctx, _) =>
                new ValueTask(Result.Fail(Error.TooManyRequests()).ToProblemDetails().ExecuteAsync(ctx.HttpContext));

            options.AddPolicy(PreKeyFetchPolicy, PreKeyFetchPartition);
        });
    }

    private static RateLimitPartition<string> PreKeyFetchPartition(HttpContext ctx)
    {
        var multiplexer = ctx.RequestServices.GetRequiredService<IConnectionMultiplexer>();

        var accessKey = Guid.TryParse(ctx.Request.Query["accessKey"], out var parsed) ? parsed : Guid.Empty;
        var caller = $"keys:session:caller:{ctx.User.GetDeviceId()}";
        var target = $"keys:session:target:{accessKey}";

        return RateLimitPartition.Get($"{caller}|{target}", _ => RateLimiter.CreateChained(
            SlidingWindow(caller, PreKeyFetchPerCaller, multiplexer),
            SlidingWindow(target, PreKeyFetchPerTarget, multiplexer)));
    }

    private static RedisSlidingWindowRateLimiter<string> SlidingWindow(string key, int permitLimit, IConnectionMultiplexer multiplexer) =>
        new(key, new RedisSlidingWindowRateLimiterOptions
        {
            ConnectionMultiplexerFactory = () => multiplexer,
            PermitLimit = permitLimit,
            Window = PreKeyFetchWindow,
        });
}
