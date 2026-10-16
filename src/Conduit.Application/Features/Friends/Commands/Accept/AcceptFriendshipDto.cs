namespace Conduit.Application.Features.Friends.Commands.Accept;

public sealed record AcceptFriendshipDto
{
    public required string Username { get; init; }
    public required string Tag { get; init; }
    public string? DisplayName { get; init; }
    public Guid? AvatarKey { get; init; }
    public IReadOnlyCollection<Guid> ConnectionIds { get; init; } = [];
}
