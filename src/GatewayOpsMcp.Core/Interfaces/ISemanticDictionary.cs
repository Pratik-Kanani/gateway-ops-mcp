using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;

public interface ISemanticDictionary
{
    IEnumerable<SemanticEntity> GetEntities();
}