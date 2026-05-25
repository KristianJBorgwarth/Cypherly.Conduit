public sealed class Avatar
{
    public Stream Content { get; init; } 
    public string? ContentType { get; init; }
    public required string Etag { get; init; }

    public bool IsModified() => Content != null;
}
