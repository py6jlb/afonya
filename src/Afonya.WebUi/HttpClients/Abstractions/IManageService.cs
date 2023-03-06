namespace Afonya.WebUi.HttpClients.Abstractions;

public interface IManageService
{
    Task<object?> GetStatus();
    Task<bool> StartBot();
    Task<bool> StopBoot();
}