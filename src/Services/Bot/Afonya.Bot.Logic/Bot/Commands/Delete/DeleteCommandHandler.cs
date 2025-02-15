﻿using Afonya.Bot.Domain.Repositories;
using Afonya.Bot.Interfaces.Services;
using MediatR;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Bot.Commands.Delete;

public class DeleteCommandHandler : IRequestHandler<DeleteCommand, bool>
{
    private readonly IMoneyTransactionRepository _moneyTransaction;
    private readonly ITelegramBotClient _botClient;
    private readonly IBotKeyboardService _botKeyboard;

    public DeleteCommandHandler(IMoneyTransactionRepository moneyTransaction, ITelegramBotClient botClient, IBotKeyboardService botKeyboard)
    {
        _moneyTransaction = moneyTransaction;
        _botClient = botClient;
        _botKeyboard = botKeyboard;
    }

    public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        
        var deleted = false;
        if (request.CallbackData.Confirm)
        {
            var res = _moneyTransaction.Delete(request.CallbackData.DataId);
            deleted = res;
        }

        if (deleted)
        {
            await _botClient.AnswerCallbackQuery(request.CallbackQueryId, $"Запись удалена", cancellationToken: cancellationToken);
            await _botClient.DeleteMessage(request.ChatId, request.MessageId, cancellationToken: cancellationToken);
        }
        else
        {
            var deleteKeyboard = _botKeyboard.GetDeleteKeyboard(request.CallbackData.DataId, request.CallbackData.OriginalMessageText);
            await _botClient.EditMessageText(request.ChatId,
                request.MessageId, request.CallbackData.OriginalMessageText, replyMarkup: deleteKeyboard, cancellationToken: cancellationToken);
        }

        return true;
    }
}