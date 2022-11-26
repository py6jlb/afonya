using Telegram.Bot.Types;

namespace MoneyBot.Interfaces.Services;

public interface IHandleUpdateService
{
    Task HandleUpdate(Update update);
}