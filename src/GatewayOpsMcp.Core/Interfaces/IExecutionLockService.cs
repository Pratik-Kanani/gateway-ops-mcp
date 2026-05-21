namespace GatewayOpsMcp.Core.Interfaces;

public interface IExecutionLockService
{
    Task<bool> AcquireAsync(
        string key,
        TimeSpan expiry);

    Task ReleaseAsync(
        string key);
}