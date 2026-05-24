namespace Conduit.Infrastructure.Settings;

public sealed record ValkeySettings
{
    public required string Host { get; init; }
    public required int Port { get; init; }
}
