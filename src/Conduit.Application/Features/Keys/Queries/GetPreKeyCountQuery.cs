using Conduit.Application.Abstractions;
using Conduit.Application.Features.Keys.Dtos;

namespace Conduit.Application.Features.Keys.Queries;

public sealed record GetPreKeyCountQuery : IQuery<PreKeyCountDto>;
