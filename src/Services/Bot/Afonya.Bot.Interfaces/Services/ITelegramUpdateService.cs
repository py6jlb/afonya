using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services;

public interface ITelegramUpdateService
{
    Task HandleUpdateAsync(Update update, CancellationToken ct = default);
}