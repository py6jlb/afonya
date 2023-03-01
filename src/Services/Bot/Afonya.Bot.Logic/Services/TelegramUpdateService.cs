using System.Globalization;
using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Services;
using Common.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.Services;

public class TelegramUpdateService : ITelegramUpdateService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramUpdateService> _logger;
    private readonly IMoneyTransactionService _moneyTransaction;
    private readonly ICategoryService _categoryService;

    public TelegramUpdateService(ITelegramBotClient botClient,
        ILogger<TelegramUpdateService> logger,
        IMoneyTransactionService moneyTransaction, ICategoryService categoryService)
    {
        _botClient = botClient;
        _logger = logger;
        _moneyTransaction = moneyTransaction;
        _categoryService = categoryService;
    }

    //-----------------------------Bot events handlers----------------------------------
    public async Task BotOnMessageAsync(Message message, CancellationToken ct = default)
    {
        _logger.LogInformation($"Получено сообщения типа: {message.Type}");
        if (message.Type != MessageType.Text) return;

        var action = (message.Text?.Split(' ').FirstOrDefault()?.ToLower()) switch
        {
            "/начать" => StartAsync(message, ct),
            "/start" => StartAsync(message, ct),
            "/закрыть" => CancelAsync(message, ct),
            "/cancel" => CancelAsync(message, ct),
            "/помощь" => HelpAsync(message, ct),
            "/help" => HelpAsync(message, ct),
            "/kbrm" => DeleteKeyboardAsync(message, ct),
            _ => NonCommandMessageAsync(message, ct)
        };
        await action;
    }

    public async Task BotOnCallbackQueryAsync(CallbackQuery callbackQuery, CancellationToken ct = default)
    {
        var callbackData = JsonConvert.DeserializeObject<CallbackInfo>(callbackQuery.Data);
        var categoryCollection = _categoryService.Get();
        var category = categoryCollection.First(x => x.Name == callbackData.Ctg);
        var msg = $"{callbackQuery.Message.Text}, в категории \"{category.HumanName}\" {category.Icon}";

        var data = _moneyTransaction.Get(callbackData.Id);
        if (data == null)
        {
            _logger.LogError("Отсуствуют данные для обновления. {msq}", category);
            return;
        }

        data.CategoryHumanName = category.HumanName;
        data.CategoryName = category.Name;
        data.CategoryIcon = category.Icon;
        _moneyTransaction.Update(data);

        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, $"Категория выбрана", cancellationToken: ct);
        await _botClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId,
            msg,
            replyMarkup: new InlineKeyboardMarkup(Array.Empty<InlineKeyboardButton>()), cancellationToken: ct);
    }

    public Task UnknownUpdateHandler(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    public async Task NotAllowed(Update update, CancellationToken ct = default)
    {
        var chatId = update switch
        {
            { Message: { } message }                       => message?.Chat.Id,
            { EditedMessage: { } message }                 => message?.Chat.Id,
            _                                              => null
        };

        if (chatId.HasValue)
        {
            await SendNotAllowedMessageAsync(chatId.Value, ct);
        }
    }


    //-----------------------------Message handlers----------------------------------
    private async Task StartAsync(Message message, CancellationToken ct = default)
    {
        await SendKeyboardAsync(message, ct);
        await HelpAsync(message, ct);
    }

    private async Task CancelAsync(Message message, CancellationToken ct = default)
    {
        await DeleteKeyboardAsync(message, ct);
    }

    private async Task HelpAsync(Message message, CancellationToken ct = default)
    {
        const string usage = "Что я умею:\n" +
                             "Мне нужно отправлять суммы расходов в рублях, а затем выбирать к какой категории расходов они относятся. " +
                             "Если надо сохранить приход, то перед суммой должен быть занк \"+\"";
        await _botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: usage, cancellationToken: ct);
    }

    private async Task SendNotAllowedMessageAsync(long chatId, CancellationToken ct = default)
    {
        const string msg = "Тебе не разрешали пользоваться этим ботом!";
        await _botClient.SendTextMessageAsync(chatId: chatId, text: msg, cancellationToken: ct);
    }

    private async Task SendKeyboardAsync(Message message, CancellationToken ct = default)
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[] { new KeyboardButton("/Помощь") })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Привет!",
            replyMarkup: replyKeyboardMarkup, cancellationToken: ct);
    }

    private async Task DeleteKeyboardAsync(Message message, CancellationToken ct = default)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Клавиатура удалена.",
            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: ct);
    }

    private async Task NonCommandMessageAsync(Message message, CancellationToken ct = default)
    {
        if(string.IsNullOrWhiteSpace(message.Text) || message.From == null || string.IsNullOrWhiteSpace(message.From.Username))
            return;
            
        var isIncome = message.Text.StartsWith("+");
        var msgText = isIncome ? message.Text[1..] : message.Text;
        var isFloat = float.TryParse(msgText, NumberStyles.Any, CultureInfo.InvariantCulture, out var num);
        if (isFloat && num > 0)
        {
            var savedData = new MoneyTransactionDto()
            {
                FromUserName = message.From.Username,
                Value = num,
                RegisterDate = message.Date,
                Sign = isIncome ? "+" : "-",
                TransactionDate = DateTime.Now,
                MessageId = message.MessageId,
                ChatId = message.Chat.Id
            };

            var dataId = _moneyTransaction.Insert(savedData);
            var keyboard = BuildInlineKeyboard(isIncome, dataId);
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"{savedData.Sign}{num} руб",
                replyMarkup: keyboard, cancellationToken: ct);
        }
        else
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Я не понимаю, что вы от меня хотите.", cancellationToken: ct);
        }
    }

    private InlineKeyboardMarkup BuildInlineKeyboard(bool isIncome, string savedDataId)
    {
        var categoryCollection = _categoryService.Get();
        var categoriesArrays = categoryCollection.ToArray().SplitArray(8);

        var inlineKeyboard = new InlineKeyboardMarkup(categoriesArrays.Select(arr =>
            arr.Select(x =>
            {
                var callback = new CallbackInfo { Id = savedDataId, Ctg = x.Name };
                return InlineKeyboardButton.WithCallbackData(x.Icon, callback.ToString());
            }).ToArray()
        ).ToArray());
        return inlineKeyboard;
    }
}