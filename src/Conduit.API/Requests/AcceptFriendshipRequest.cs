namespace Conduit.API.Requests;

public sealed record AcceptFriendshipRequest
{
    public required string FriendTag { get; init; }
}
