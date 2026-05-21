namespace GatewayOpsMcp.Api.Endpoints;

public static class HealthEndpoints
{
    public static RouteGroupBuilder MapHealthEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/health");

        group.MapGet(
            "/",
            () =>
            {
                return Results.Ok(
                    new
                    {
                        status = "healthy",
                        service = "GatewayOpsMcp",
                        utc = DateTime.UtcNow
                    });
            });

        return group;
    }
}