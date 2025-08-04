using Conduit.Application.Abstractions;
using Conduit.Application.Contracts;
using Conduit.Domain.Common;
using Microsoft.Extensions.Logging;

namespace Conduit.Application.Features.Social.Queries;

public sealed class GetUserProfileQueryHandler(
    IUserProfileProvider userProfileProvider,
    IDeviceProvider deviceProvider,
    ILogger<GetUserProfileQueryHandler> logger) 
    : IQueryHandler<GetUserProfileQuery, GetUserProfileDto>
{
    public Task<Result<GetUserProfileDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}