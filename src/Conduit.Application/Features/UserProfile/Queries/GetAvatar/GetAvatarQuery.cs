using Conduit.Application.Abstractions;

public sealed record GetAvatarQuery : IQuery<Avatar>
{
    public required Guid FileKey { get; init; }
}
