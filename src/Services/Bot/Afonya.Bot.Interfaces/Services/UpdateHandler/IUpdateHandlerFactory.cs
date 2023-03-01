using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services.UpdateHandler;

public interface IUpdateHandlerFactory
{
    IUpdateHandler CreateHandler(Update update);
}