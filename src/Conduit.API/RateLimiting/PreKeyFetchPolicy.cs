using System.Threading.RateLimiting;
using Conduit.API.Common;
using Microsoft.AspNetCore.RateLimiting;
using RedisRateLimiting;
using StackExchange.Redis;

namespace Conduit.API.RateLimiting;

internal sealed class PreKeyFetchPolicy(IConnectionMultiplexer multiplexer) : IRateLimiterPolicy<string>
{
    public const string Name = "prekey-fetch";

    private const int PerCaller = 200;
    private const int PerTarget = 100;
    private static readonly TimeSpan Window = TimeSpan.FromHours(1);

    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => null;

    public RateLimitPartition<string> GetPartition(HttpContext ctx)
    {
        var accessKey = Guid.TryParse(ctx.Request.Query["accessKey"], out var parsed) ? parsed : Guid.Empty;
        var caller = $"keys:session:caller:{ctx.User.GetDeviceId()}";
        var target = $"keys:session:target:{accessKey}";

        return RateLimitPartition.Get($"{caller}|{target}", _ => RateLimiter.CreateChained(
            SlidingWindow(caller, PerCaller),
            SlidingWindow(target, PerTarget)));
    }

    private RedisSlidingWindowRateLimiter<string> SlidingWindow(string key, int permitLimit) =>
        new(key, new RedisSlidingWindowRateLimiterOptions
        {
            ConnectionMultiplexerFactory = () => multiplexer,
            PermitLimit = permitLimit,
            Window = Window,
        });
}
