using Afonya.Bot.Domain.Repositories;
using Afonya.Bot.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Bot.Commands.SetCategory;

public class SetCategoryCommandHandler : IRequestHandler<SetCategoryCommand, bool>
{
    private readonly ILogger<SetCategoryCommandHandler> _logger;
    private readonly IMoneyTransactionRepository _moneyTransaction;
    private readonly ITelegramBotClient _botClient;
    private readonly IBotKeyboardService _botKeyboard;

    public SetCategoryCommandHandler(IMoneyTransactionRepository moneyTransaction, 
        ILogger<SetCategoryCommandHandler> logger, ITelegramBotClient botClient, IBotKeyboardService botKeyboard)
    {
        _moneyTransaction = moneyTransaction;
        _logger = logger;
        _botClient = botClient;
        _botKeyboard = botKeyboard;
    }

    public async Task<bool> Handle(SetCategoryCommand request, CancellationToken cancellationToken)
    {
        var parts = request.MessageText.Split(": ").Skip(1);
        var msg = $"{string.Join(": ", parts)}, в категории \"{request.CallbackData.Category.HumanName}\" {request.CallbackData.Category.Icon}";
        var data = _moneyTransaction.Get(request.CallbackData.DataId);

        if (data == null)
        {
            _logger.LogError("Отсутствуют данные для обновления. {msq}", request.CallbackData.Category);
            return false;
        }

        data.SetCategory(request.CallbackData.Category.Name, request.CallbackData.Category.Icon, request.CallbackData.Category.HumanName);
        _moneyTransaction.Update(data);

        await _botClient.AnswerCallbackQuery(request.CallbackQueryId, $"Категория выбрана", cancellationToken: cancellationToken);
        var deleteKeyboard = _botKeyboard.GetDeleteKeyboard(request.CallbackData.DataId, msg);
        await _botClient.EditMessageText(request.ChatId, request.MessageId, msg, replyMarkup: deleteKeyboard, cancellationToken: cancellationToken);
        return true;
    }
}