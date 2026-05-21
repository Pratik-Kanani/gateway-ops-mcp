namespace GatewayOpsMcp.Core.Models;

public class ValidationResult
{
    public bool Valid { get; set; }

    public List<string> Errors { get; set; }

    public ValidationResult()
    {
        Errors = [];
    }
}