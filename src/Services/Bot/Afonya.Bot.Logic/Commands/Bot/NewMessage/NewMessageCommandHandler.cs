using System.Globalization;
using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services;
using MediatR;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Commands.Bot.NewMessage;

public class NewMessageCommandHandler : IRequestHandler<NewMessageCommand, bool>
{
    private readonly IMoneyTransactionRepository _moneyTransaction;
    private readonly IBotKeyboardService _botKeyboard;
    private readonly ITelegramBotClient _botClient;

    public NewMessageCommandHandler(IMoneyTransactionRepository moneyTransaction, 
        IBotKeyboardService botKeyboard, ITelegramBotClient botClient)
    {
        _moneyTransaction = moneyTransaction;
        _botKeyboard = botKeyboard;
        _botClient = botClient;
    }

    public async Task<bool> Handle(NewMessageCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.MessageText)) return false;

        var (isIncome, num) = GetNumber(request.MessageText);

        if (num is null or 0)
        {
            const string msg = "Я не понимаю, что вы от меня хотите.";
            await _botClient.SendTextMessageAsync(chatId: request.ChatId, text: msg, cancellationToken: cancellationToken);
            return true;
        }

        var savedData = new MoneyTransaction(
            num.Value, request.MessageId, request.ChatId, isIncome ? "+" : "-",
            null, null, null, request.MessageDate, DateTime.Now, request.UserName);

        var dataId = _moneyTransaction.Insert(savedData);
        var keyboard = _botKeyboard.GetCategoryKeyboard(isIncome, dataId);
        await _botClient.SendTextMessageAsync(chatId: request.ChatId, text: $"Выберете категорию для: {savedData.Sign}{num} руб", replyMarkup: keyboard, cancellationToken: cancellationToken);
        await _botClient.DeleteMessageAsync(request.ChatId, request.MessageId, cancellationToken: cancellationToken);
        return true;
    }

    private static (bool, float?) GetNumber(string numText)
    {
        var text = numText.Trim().Replace(',', '.');
        var isIncome = text.StartsWith("+");
        var withMinus = text.StartsWith("-");
        var msgText = isIncome || withMinus ? text[1..] : text;

        var isFloat = float.TryParse(msgText, NumberStyles.Float, CultureInfo.InvariantCulture, out var num);
        return isFloat ? (isIncome, num) : (isIncome, null);
    }
}