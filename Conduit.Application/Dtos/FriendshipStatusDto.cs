using Conduit.Domain.Enums;
// ReSharper disable ConvertToPrimaryConstructor

namespace Conduit.Application.Dtos;

public sealed record FriendshipStatusDto
{
    public FriendshipStatus? Status { get; init; }
    public FriendshipDirection Direction { get; init; }
}