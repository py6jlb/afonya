namespace Afonya.Bot.Interfaces.Pooling;

public interface IReceiverService
{
    Task ReceiveAsync(CancellationToken stoppingToken = default);
}