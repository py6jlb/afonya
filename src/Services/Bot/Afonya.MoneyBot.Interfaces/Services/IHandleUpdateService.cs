using Telegram.Bot.Types;

namespace Afonya.MoneyBot.Interfaces.Services;

public interface IHandleUpdateService
{
    Task HandleUpdate(Update update);
}