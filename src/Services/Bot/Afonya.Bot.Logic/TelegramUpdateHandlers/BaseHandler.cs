using Afonya.Bot.Interfaces.Services.UpdateHandler;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.UpdateHandlers;

public abstract class BaseHandler : IUpdateHandler
{
    protected readonly ILogger Logger;
    protected readonly ITelegramBotClient BotClient;

    protected BaseHandler(ILogger logger, ITelegramBotClient botClient)
    {
        Logger = logger;
        BotClient = botClient;
    }

    public virtual async Task HandleAsync(Update update, long chatId, CancellationToken ct = default)
    {
        Logger.LogInformation("Неизвестный тип обновления: {UpdateType}", update.Type);
        const string msg = "Я не понимаю, что вы от меня хотите.";
        await BotClient.SendTextMessageAsync(chatId: chatId, text: msg, cancellationToken: ct);
    }

    public virtual async Task NotAllowed(long chatId, CancellationToken ct = default)
    {
        const string msg = "Ты кто такой? Давай до свидания.";
        await BotClient.SendTextMessageAsync(chatId: chatId, text: msg, cancellationToken: ct);
    }
}