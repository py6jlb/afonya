using System.Globalization;
using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Logic.UpdateHandlers;
using Common.Extensions;
using Microsoft.Extensions.Logging;
using Shared.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.TelegramUpdateHandlers;

public class MessageHandler : BaseHandler
{
    private readonly IMoneyTransactionService _moneyTransaction;
    private readonly ICategoryService _categoryService;

    public MessageHandler(ILogger<MessageHandler> logger, ITelegramBotClient botClient, 
        IMoneyTransactionService moneyTransaction, ICategoryService categoryService) : base(logger, botClient)
    {
        _moneyTransaction = moneyTransaction;
        _categoryService = categoryService;
    }

    public override async Task HandleAsync(Update update, long chatId, CancellationToken ct = default)
    {
        Logger.LogInformation("Получено обновления типа: Message");

        var message = update.Message!;
        if (message.Type != MessageType.Text) return;

        var action = (message.Text?.Split(' ').FirstOrDefault()?.ToLower()) switch
        {
            "/начать" or "/start"  => StartAsync(message, ct),
            "/закрыть" or "/cancel" => DeleteKeyboardAsync(message, ct),
            "/помощь" or "/help" => HelpAsync(message, ct),
            "/kbrm" => DeleteKeyboardAsync(message, ct),
            _ => NonCommandMessageAsync(message, ct)
        };
        await action;
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
    
    private async Task DeleteKeyboardAsync(Message message, CancellationToken ct = default)
    {
        await BotClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Клавиатура удалена.",
            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: ct);
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

        var savedData = new MoneyTransactionDto()
        {
            FromUserName = message.From.Username,
            Value = num.Value,
            RegisterDate = message.Date,
            Sign = isIncome ? "+" : "-",
            TransactionDate = DateTime.Now,
            MessageId = message.MessageId,
            ChatId = message.Chat.Id
        };

        var dataId = _moneyTransaction.Insert(savedData);
        var keyboard = BuildCallbackCategoryKeyboard(isIncome, dataId);
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

    private InlineKeyboardMarkup BuildCallbackCategoryKeyboard(bool isIncome, string savedDataId)
    {
        var categoryCollection = _categoryService.Get();
        var categoriesArrays = categoryCollection.ToArray().SplitArray(7);

        var inlineKeyboard = new InlineKeyboardMarkup(categoriesArrays.Select(arr =>
            arr.Select(x =>
            {
                var callback = new CallbackInfo { Id = savedDataId, Ctg = x.Name };
                var button = InlineKeyboardButton.WithCallbackData(x.Icon, callback.ToString());
                return button;
            }).ToArray()
        ).ToArray());
        return inlineKeyboard;
    }
}