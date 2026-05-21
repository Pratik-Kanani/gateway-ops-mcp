using GatewayOpsMcp.Core.Models;

namespace GatewayOpsMcp.Core.Interfaces;
public interface IPendingActionService
{
    string Sign(
        PendingAction action);

    bool Verify(
        PendingAction action, 
        string signature);
}