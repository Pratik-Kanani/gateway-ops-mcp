namespace GatewayOpsMcp.Core.Models;

public class ToolMetadata
{
    public string Description { get; set; }

    public List<string> Keywords { get; set; }

    public List<string> Examples { get; set; }
    public List<string> IntentPhrases { get; set; }

    public ToolMetadata()
    {
        Description = string.Empty;

        Keywords = [];

        Examples = [];
        IntentPhrases = [];
    }
}