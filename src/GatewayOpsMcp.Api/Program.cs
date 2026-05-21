using GatewayOpsMcp.Api.Endpoints;
using GatewayOpsMcp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// SERVICES
// --------------------------------------------------

builder.Services.AddGatewayAuthentication(
    builder.Configuration);

builder.Services.AddGatewayInfrastructure(
    builder.Configuration);

builder.Services.AddGatewayTools();

builder.Services.AddGatewayObservability();

// --------------------------------------------------

var app = builder.Build();

// --------------------------------------------------
// MIDDLEWARE
// --------------------------------------------------

app.UseGatewayMiddleware();

// --------------------------------------------------
// ENDPOINTS
// --------------------------------------------------

app.MapMcpEndpoints();

app.MapToolEndpoints();

app.MapHealthEndpoints();

// --------------------------------------------------

app.Run("http://0.0.0.0:5000");