using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services;

public interface IBotManagementService
{
    Task StartAsync(CancellationToken ct = default);
    Task StopAsync(CancellationToken ct = default);
    Task<WebhookInfo> StatusAsync(CancellationToken ct = default);
}