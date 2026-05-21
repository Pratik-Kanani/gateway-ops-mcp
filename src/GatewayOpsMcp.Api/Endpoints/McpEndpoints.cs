using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;
using GatewayOpsMcp.Core.Orchestration;

namespace GatewayOpsMcp.Api.Endpoints;

public static class McpEndpoints
{
    public static RouteGroupBuilder MapMcpEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/mcp");

        group.MapPost(
            "/",
            HandleMcpAsync)
            .RequireAuthorization();

        return group;
    }

    private static async Task<IResult> HandleMcpAsync(
        HttpContext httpContext,
        McpRequest request,
        IMcpOrchestrator orchestrator)
    {
        if (httpContext.Items["ctx"] is not RequestContext ctx)
        {
            return Results.Unauthorized();
        }

        var result =
            await orchestrator.HandleAsync(
                request,
                ctx);

        return Results.Ok(result);
    }
}