namespace Conduit.API.Requests;

public sealed record CreateFriendshipRequest
{
    public required string FriendTag { get; init; }
}