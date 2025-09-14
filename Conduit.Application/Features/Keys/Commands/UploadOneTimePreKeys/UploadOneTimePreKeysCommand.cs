
using Conduit.Application.Abstractions;
using Conduit.Domain.Models;

namespace Conduit.Application.Features.Keys.Commands.UploadOneTimePreKeys;

public sealed record UploadOneTimePreKeysCommand : ICommand
{
    public required IReadOnlyCollection<PreKey> PreKeys { get; init; }
}