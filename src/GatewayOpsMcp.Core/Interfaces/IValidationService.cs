using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface IValidationService
{
    ValidationResult Validate(
        ToolExecutionContext context);
}