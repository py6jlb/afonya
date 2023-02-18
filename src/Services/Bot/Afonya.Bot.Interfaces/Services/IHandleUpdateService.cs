using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services;

public interface IHandleUpdateService
{
    Task HandleUpdate(Update update);
}