using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services.UpdateHandler;

public interface IUpdateHandler
{
    Task HandleAsync(Update msg, long chatId, CancellationToken ct = default);
    Task NotAllowed(long chatId, CancellationToken ct = default);
}