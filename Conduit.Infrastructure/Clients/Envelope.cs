namespace Conduit.Infrastructure.Clients;

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