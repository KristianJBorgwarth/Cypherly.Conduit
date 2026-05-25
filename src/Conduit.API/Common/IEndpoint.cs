namespace Conduit.API.Endpoints;

internal interface IEndpoint
{
    void MapRoutes(IEndpointRouteBuilder routeBuilder);
}