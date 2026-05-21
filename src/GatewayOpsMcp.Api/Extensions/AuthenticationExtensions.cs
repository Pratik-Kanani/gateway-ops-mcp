using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GatewayOpsMcp.Api.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection
        AddGatewayAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
    {
        services
            .AddAuthentication("Bearer")
            .AddJwtBearer(
                "Bearer",
                options =>
                {
                    options.TokenValidationParameters =
                        new()
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,

                            // DEV ONLY
                            ValidateLifetime = false,

                            ValidateIssuerSigningKey = true,

                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.UTF8.GetBytes(
                                        configuration["Jwt:Secret"]!))
                        };
                });

        services.AddAuthorization();

        return services;
    }
}