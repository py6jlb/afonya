using Telegram.Bot.Types;

namespace Afonya.Bot.Interfaces.Services;

public interface IHandleUpdateService
{
    Task HandleUpdateAsync(Update update, CancellationToken ct = default);
}