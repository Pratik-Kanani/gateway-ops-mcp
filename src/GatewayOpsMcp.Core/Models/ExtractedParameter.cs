namespace GatewayOpsMcp.Core.Models;

public class ExtractedParameter
{
    public object? Value { get; set; }

    public double Confidence { get; set; }

    public string Source { get; set; }

    public string? RawValue { get; set; }

    public ExtractedParameter()
    {
        Source = string.Empty;
    }
}