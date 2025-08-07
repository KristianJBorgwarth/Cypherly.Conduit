namespace Conduit.Infrastructure.Clients;

internal class Envelope<T>
{
    public T? Result { get; init; }
    public string? ErrorMessage { get; init; }
    public required DateTime TimeGenerated { get; init; }
}