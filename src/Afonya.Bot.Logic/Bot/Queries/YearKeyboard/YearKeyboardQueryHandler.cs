using Afonya.Bot.Interfaces.Services;
using MediatR;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Bot.Queries.YearKeyboard;

public class YearKeyboardQueryHandler : IRequestHandler<YearKeyboardQuery, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IBotKeyboardService _botKeyboard;
    public YearKeyboardQueryHandler(ITelegramBotClient botClient, IBotKeyboardService botKeyboard)
    {
        _botClient = botClient;
        _botKeyboard = botKeyboard;
    }

    public async Task<bool> Handle(YearKeyboardQuery request, CancellationToken cancellationToken)
    {        
       var keyboard = _botKeyboard.GetYearKeyboard(request.OriginalMessageId, request.Year.Value);
       await _botClient.EditMessageReplyMarkup(request.ChatId, request.OriginalMessageId, replyMarkup: keyboard, cancellationToken: cancellationToken);
       return true;
    }
}
