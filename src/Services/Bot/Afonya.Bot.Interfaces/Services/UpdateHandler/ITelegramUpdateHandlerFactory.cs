using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services.UpdateHandler;

public interface ITelegramUpdateHandlerFactory
{
    ITelegramUpdateHandler CreateHandler(Update update);
}