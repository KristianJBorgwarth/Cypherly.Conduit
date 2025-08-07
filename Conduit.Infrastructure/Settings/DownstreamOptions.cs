namespace Conduit.Infrastructure.Settings;

internal sealed class DownstreamOptions
{
    public List<DownStream> DownStreams { get; init; } = [];

    public DownStream GetDownStream(string name)
    {
        return DownStreams.FirstOrDefault(d => d.Name == name) 
               ?? throw new ArgumentException($"{name} is not a valid DownStream");
    }
}

internal sealed record DownStream
{
    public required string Name { get; init; } = null!;
    public required string Url { get; init; } = null!;
    public required bool UsesAuthentication { get; init; } = false;
}
