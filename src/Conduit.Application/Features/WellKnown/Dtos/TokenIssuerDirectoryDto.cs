using System.Text.Json.Serialization;

namespace Conduit.Application.Features.WellKnown.Dtos;

public sealed record TokenIssuerDirectoryDto
{
    [JsonPropertyName("issuer-request-uri")]
    public required string IssuerRequestUri { get; init; }

    [JsonPropertyName("token-keys")]
    public required IReadOnlyList<TokenKeyDto> TokenKeys { get; init; }
}

public sealed record TokenKeyDto
{
    [JsonPropertyName("token-type")]
    public required ushort TokenType { get; init; }

    [JsonPropertyName("token-key")]
    public required string TokenKey { get; init; }
}
