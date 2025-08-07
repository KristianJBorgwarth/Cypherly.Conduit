namespace Conduit.Infrastructure.Settings;

internal sealed class DownstreamOptions
{
    private readonly List<DownStream> _downStreams = [];

    public DownStream GetDownStream(string name)
    {
        return _downStreams.FirstOrDefault(d => d.Name == name) 
            ?? throw new ArgumentException($"{name} is not a valid DownStream");
    }
}

internal sealed record DownStream
{
    public required string Name { get; init; } = null!;
    public required string Url { get; init; } = null!;
    public required bool UsesAuthentication { get; init; } = false;
}
