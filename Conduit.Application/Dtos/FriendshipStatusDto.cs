using Conduit.Domain.Enums;
// ReSharper disable ConvertToPrimaryConstructor

namespace Conduit.Application.Dtos;

public sealed class FriendshipStatusDto
{
    public FriendshipStatus Status { get; private init; }
    public FriendshipDirection Direction { get; private init; }
    
    public FriendshipStatusDto(FriendshipStatus status, FriendshipDirection direction)
    {
        Status = status;
        Direction = direction;
    }
}