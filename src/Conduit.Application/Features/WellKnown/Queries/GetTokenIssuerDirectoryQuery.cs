using Conduit.Application.Abstractions;
using Conduit.Application.Features.WellKnown.Dtos;

namespace Conduit.Application.Features.WellKnown.Queries;

public sealed record GetTokenIssuerDirectoryQuery : IQuery<TokenIssuerDirectoryDto>;
