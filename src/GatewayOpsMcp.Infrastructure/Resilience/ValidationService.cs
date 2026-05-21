using GatewayOpsMcp.Core.Interfaces;
using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Infrastructure.Resilience;

public class ValidationService : IValidationService
{
    private readonly IToolRegistry _registry;

    public ValidationService(
        IToolRegistry registry)
    {
        _registry = registry;
    }

    public ValidationResult Validate(
        ToolExecutionContext context)
    {
        var tool = _registry.GetByName(context.ToolName);

        var schema = tool.Schema;

        var result = new ValidationResult
        {
            Valid = true
        };

        foreach (var param in schema.Parameters)
        {
            var exists = context.Parameters
                .TryGetValue(param.Name, out var value);

            // required check
            if (param.Required && !exists)
            {
                result.Valid = false;

                result.Errors.Add(
                    $"{param.Name} is required");

                continue;
            }

            if (!exists || value == null)
                continue;

            // number validation
            if (param.Type == "number")
            {
                if (!decimal.TryParse(
                        value.Value?.ToString(),
                        out var number))
                {
                    result.Valid = false;

                    result.Errors.Add(
                        $"{param.Name} must be numeric");

                    continue;
                }

                if (param.Min.HasValue &&
                    number < param.Min.Value)
                {
                    result.Valid = false;

                    result.Errors.Add(
                        $"{param.Name} below minimum");
                }

                if (param.Max.HasValue &&
                    number > param.Max.Value)
                {
                    result.Valid = false;

                    result.Errors.Add(
                        $"{param.Name} exceeds maximum");
                }
            }
            if (value.Confidence < 0.70)
            {
                result.Valid = false;

                result.Errors.Add(
                    $"{param.Name} confidence too low");
            }
        }

        return result;
    }
}