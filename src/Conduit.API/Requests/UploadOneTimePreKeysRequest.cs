using Conduit.Domain.Models;

namespace Conduit.API.Requests;

public sealed record UploadOneTimePreKeysRequest
{
    public required IReadOnlyCollection<PreKey>  PreKeys { get; init; }
}