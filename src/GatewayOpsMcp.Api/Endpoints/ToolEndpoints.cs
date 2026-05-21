using GatewayOpsMcp.Core.Interfaces;

namespace GatewayOpsMcp.Api.Endpoints;

public static class ToolEndpoints
{
    public static RouteGroupBuilder MapToolEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/tools");

        group.MapGet(
            "/",
            GetTools);

        group.MapGet(
            "/capabilities/{category}",
            GetToolsByCategory);

        group.MapGet(
            "/graph",
            GetToolGraph);

        return group;
    }

    private static IResult GetTools(
        IToolDiscoveryService discovery)
    {
        var tools = discovery.GetTools();

        return Results.Ok(tools);
    }

    private static IResult GetToolsByCategory(
        string category,
        IToolRegistry registry)
    {
        var tools =
            registry
                .GetAll()
                .Where(tool =>
                    tool.Capabilities.Any(c =>
                        c.Category.Equals(
                            category,
                            StringComparison.OrdinalIgnoreCase)))
                .Select(tool => new
                {
                    tool.Name,
                    tool.Capabilities
                });

        return Results.Ok(tools);
    }

    private static IResult GetToolGraph(
        IToolRegistry registry)
    {
        var graph =
            registry
                .GetAll()
                .Select(tool => new
                {
                    tool.Name,
                    tool.Stage,
                    tool.Dependencies
                });

        return Results.Ok(graph);
    }
}