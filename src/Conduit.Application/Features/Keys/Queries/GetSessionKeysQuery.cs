using Conduit.Application.Abstractions;
using Conduit.Application.Features.Keys.Dtos;

namespace Conduit.Application.Features.Keys.Queries;

public sealed record GetSessionKeysQuery : IQuery<SessionKeysDto>
{
    public required Guid AccessKey { get; init; }
}
