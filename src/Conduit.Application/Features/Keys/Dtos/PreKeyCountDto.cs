namespace Conduit.Application.Features.Keys.Dtos;

public sealed record PreKeyCountDto
{
    public required int KeyCount { get; init; }
}
