using System.Globalization;
using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.TelegramUpdateHandlers;

public class MessageHandler : BaseHandler
{
    private readonly IMoneyTransactionRepository _moneyTransaction;
    private readonly IBotKeyboardService _botKeyboard;

    public MessageHandler(ILogger<MessageHandler> logger, ITelegramBotClient botClient, 
        IMoneyTransactionRepository moneyTransaction, IBotKeyboardService botKeyboard) : base(logger, botClient)
    {
        _moneyTransaction = moneyTransaction;
        _botKeyboard = botKeyboard;
    }

    public override async Task HandleAsync(Update update, long chatId, CancellationToken ct = default)
    {
        Logger.LogInformation("Получено обновления типа: Message");

        var message = update.Message!;
        if (message.Type != MessageType.Text) return;

        var action = (message.Text?.Split(' ').FirstOrDefault()?.ToLower()) switch
        {
            "/начать" or "/start"  => StartAsync(message, ct),
            "/закрыть" or "/cancel" => RemoveKeyboardAsync(message, ct),
            "/помощь" or "/help" => HelpAsync(message, ct),
            "/kbrm" => RemoveKeyboardAsync(message, ct),
            _ => NonCommandMessageAsync(message, ct)
        };
        await action;
    }

    private async Task NonCommandMessageAsync(Message message, CancellationToken ct = default)
    {
        if(string.IsNullOrWhiteSpace(message.Text) || message.From == null || string.IsNullOrWhiteSpace(message.From.Username))
            return;

        var (isIncome, num) = GetNumber(message.Text);

        if (num is null or 0)
        {
            const string msg = "Я не понимаю, что вы от меня хотите.";
            await BotClient.SendTextMessageAsync(chatId: message.Chat.Id, text: msg, cancellationToken: ct);
            return;
        }

        var savedData = new MoneyTransaction(
            num.Value, message.MessageId, message.Chat.Id, isIncome ? "+" : "-",
            null, null, null, message.Date, DateTime.Now, message.From.Username);

        var dataId = _moneyTransaction.Insert(savedData);
        var keyboard = _botKeyboard.GetCategoryKeyboard(isIncome, dataId);
        await BotClient.SendTextMessageAsync(chatId: message.Chat.Id, text: $"{savedData.Sign}{num} руб", replyMarkup: keyboard, cancellationToken: ct);
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

    private async Task StartAsync(Message message, CancellationToken ct = default)
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[] { new KeyboardButton("/Помощь") }) { ResizeKeyboard = true};
        await BotClient.SendTextMessageAsync(chatId: message.Chat.Id, text: "Привет!", replyMarkup: replyKeyboardMarkup, cancellationToken: ct);
        await HelpAsync(message, ct);
    }
    
    private async Task HelpAsync(Message message, CancellationToken ct = default)
    {
        const string usage = "Что я умею:\n" +
                             "Мне нужно отправлять суммы расходов в рублях, а затем выбирать к какой категории расходов они относятся. " +
                             "Если надо сохранить приход, то перед суммой должен быть занк \"+\"";
        await BotClient.SendTextMessageAsync(chatId: message.Chat.Id, text: usage, cancellationToken: ct);
    }
    
    private async Task RemoveKeyboardAsync(Message message, CancellationToken ct = default)
    {
        await BotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Клавиатура удалена.",
            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: ct);
    }
}