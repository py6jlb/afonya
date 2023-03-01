using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.UpdateHandlers;

public class EditedMessageHandler : BaseHandler
{
    public EditedMessageHandler(ILogger logger, ITelegramBotClient botClient) : base(logger, botClient)
    {
    }

    public override Task HandleAsync(Update update, long chatId, CancellationToken ct = default)
    {
        return base.HandleAsync(update, chatId, ct);
    }
}