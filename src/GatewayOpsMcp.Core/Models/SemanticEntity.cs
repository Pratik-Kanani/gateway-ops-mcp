namespace GatewayOpsMcp.Core.Models;

public class SemanticEntity
{
    public string Name { get; set; }

    public string CanonicalValue { get; set; }

    public List<string> Synonyms { get; set; }

    public SemanticEntity()
    {
        Name = string.Empty;

        CanonicalValue = string.Empty;

        Synonyms = [];
    }
}