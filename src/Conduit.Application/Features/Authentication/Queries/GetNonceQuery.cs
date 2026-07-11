using Conduit.Application.Abstractions;

namespace Conduit.Application.Features.Authentication.Queries;

public sealed record GetNonceQuery : IQuery<GetNonceDto>
{
    public required Guid UserId { get; init; }
    public required Guid DeviceId { get; init; }
}
