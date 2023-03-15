using Afonya.Bot.Interfaces.Services;
using MediatR;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Queries.Bot.ConfirmDelete;

public class ConfirmDeleteQueryHandler : IRequestHandler<ConfirmDeleteQuery, bool>
{
    private readonly IBotKeyboardService _keyboardService;
    private readonly ITelegramBotClient _botClient;
    public ConfirmDeleteQueryHandler(IBotKeyboardService keyboardService, ITelegramBotClient botClient)
    {
        _keyboardService = keyboardService;
        _botClient = botClient;
    }

    public async Task<bool> Handle(ConfirmDeleteQuery request, CancellationToken cancellationToken)
    {
        
        var confirmKeyboard = _keyboardService.GetDeleteConfirmKeyboard(request.CallbackData);
        var msg = $"Вы действительно хотите удалить запись: {request.MessageText}";
        await _botClient.EditMessageTextAsync(request.ChatId, request.MessageId, msg, replyMarkup: confirmKeyboard, cancellationToken: cancellationToken);
        return true;
    }
}