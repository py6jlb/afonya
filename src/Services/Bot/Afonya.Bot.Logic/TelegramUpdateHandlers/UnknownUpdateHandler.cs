using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.TelegramUpdateHandlers;

public class UnknownUpdateHandler : BaseHandler
{
    public UnknownUpdateHandler(ILogger logger, ITelegramBotClient botClient) : base(logger, botClient)
    {
    }

    public override Task HandleAsync(Update update, long chatId, CancellationToken ct = default)
    {
        Logger.LogInformation("Получено обновления типа: UnknownUpdate");
        return base.HandleAsync(update, chatId, ct);
    }
}