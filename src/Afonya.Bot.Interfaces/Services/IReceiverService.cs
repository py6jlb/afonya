namespace Afonya.Bot.Interfaces.Services;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken = default);
}