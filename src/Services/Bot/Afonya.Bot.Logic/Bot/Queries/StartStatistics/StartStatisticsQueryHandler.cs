using Afonya.Bot.Interfaces.Services;
using MediatR;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Bot.Queries.StartStatistics;

public class StartStatisticsQueryHandler : IRequestHandler<StartStatisticQuery, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IBotKeyboardService _botKeyboard;

    public StartStatisticsQueryHandler(ITelegramBotClient botClient, IBotKeyboardService botKeyboard)
    {
        _botClient = botClient;
        _botKeyboard = botKeyboard;
    }

    public async Task<bool> Handle(StartStatisticQuery request, CancellationToken cancellationToken)
    {
        
        var msg = await _botClient.SendMessage(request.ChatId, "Выберете период для расчета статистики", cancellationToken: cancellationToken);
        var keyboard = _botKeyboard.GetYearKeyboard(msg.MessageId, DateTime.Now.Year);
        await _botClient.EditMessageReplyMarkup(request.ChatId, msg.MessageId, replyMarkup: keyboard, cancellationToken: cancellationToken);
        return true;
    }
}
