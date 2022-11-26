using Telegram.Bot.Types;

namespace Bot.Interfaces.Services;

public interface IHandleUpdateService
{
    Task HandleUpdate(Update update);
}