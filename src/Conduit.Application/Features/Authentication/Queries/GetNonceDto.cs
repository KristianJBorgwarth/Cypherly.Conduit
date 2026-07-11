namespace Conduit.Application.Features.Authentication.Queries;

public sealed record GetNonceDto
{
    public required Guid NonceId { get; init; }
    public required Guid DeviceId { get; init; }
    public required string NonceValue { get; init; }
}
