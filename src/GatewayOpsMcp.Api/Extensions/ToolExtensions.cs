using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Infrastructure.Services;
using GatewayOpsMcp.Tools.Implementations;

namespace GatewayOpsMcp.Api.Extensions;

public static class ToolExtensions
{
    public static IServiceCollection
        AddGatewayTools(
            this IServiceCollection services)
    {
        services.AddSingleton<
            CreatePaymentLinkTool>();

        services.AddSingleton<
            DiagnosePaymentIssueTool>();

        services.AddSingleton<
            IToolRegistry,
            ToolRegistry>();

        return services;
    }
}