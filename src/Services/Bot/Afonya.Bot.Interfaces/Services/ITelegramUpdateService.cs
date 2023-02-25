using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services;

public interface ITelegramUpdateService
{
    Task BotOnMessageAsync(Message message, CancellationToken ct = default);
    Task BotOnCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken ct = default);
    Task UnknownUpdateHandler(Update update);
}