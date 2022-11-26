using System.Globalization;
using Afonya.MoneyBot.Interfaces.Dto;
using Afonya.MoneyBot.Interfaces.Services;
using Common.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Contracts;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.MoneyBot.Logic.Services;

public class HandleUpdateService : IHandleUpdateService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<HandleUpdateService> _logger;
    private readonly IMoneyTransactionService _moneyTransaction;
    private readonly ICategoryService _categoryService;

    public HandleUpdateService(ITelegramBotClient botClient,
        ILogger<HandleUpdateService> logger,
        IMoneyTransactionService moneyTransaction, ICategoryService categoryService)
    {
        _botClient = botClient;
        _logger = logger;
        _moneyTransaction = moneyTransaction;
        _categoryService = categoryService;
    }

    public async Task HandleUpdate(Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => HandleMessageAsync(update.Message!),
            UpdateType.EditedMessage => HandleMessageAsync(update.EditedMessage!),
            UpdateType.CallbackQuery => HandleCallbackQuery(update.CallbackQuery!),
            // UpdateType.Unknown:
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            // UpdateType.Poll:
            //UpdateType.InlineQuery        => BotOnInlineQueryReceived(update.InlineQuery!),
            //UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult!),
            _ => UnknownUpdateHandlerAsync(update)
        };

        try
        {
            await handler;
        }
#pragma warning disable CA1031
        catch (Exception exception)
#pragma warning restore CA1031
        {
            await HandleErrorAsync(exception);
        }
    }

    //-----------------------------Handlers----------------------------------
    private async Task HandleMessageAsync(Message message)
    {
        _logger.LogInformation($"Receive message type: {message.Type}");
        if (message.Type != MessageType.Text)
            return;

        var action = (message.Text?.Split(' ').First()?.ToLower()) switch
        {
            "/начать" => HandleStartAsync(message),
            "/start" => HandleStartAsync(message),
            "/закрыть" => HandleCancelAsync(message),
            "/cancel" => HandleCancelAsync(message),
            "/помощь" => HandleHelpAsync(message),
            "/help" => HandleHelpAsync(message),
            _ => HandleNonCommandMessageAsync(message)
        };
        await action;
    }

    private async Task HandleStartAsync(Message message)
    {
        await _botClient.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
        await SendBaseKeyboardAsync(message);
        await HandleHelpAsync(message);
    }

    private async Task HandleCancelAsync(Message message)
    {
        await DeleteBaseKeyboardAsync(message);
    }

    private async Task HandleHelpAsync(Message message)
    {
        const string usage = "Что я умею:\n" +
                             "Мне нужно отправлять суммы расходов в рублях, а затем выбирать к какой категории расходов они относятся. " +
                             "Если надо сохранить приход, то перед суммой должен ыть занк \"+\"";
        await _botClient.SendTextMessageAsync(chatId: message.Chat.Id, text: usage);
    }

    private async Task SendBaseKeyboardAsync(Message message)
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[] { new KeyboardButton("/Помощь") })
        {
            ResizeKeyboard = true
        };

        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Привет!",
            replyMarkup: replyKeyboardMarkup
        );
    }

    private async Task DeleteBaseKeyboardAsync(Message message)
    {
        await _botClient.SendTextMessageAsync(
            chatId: message.Chat.Id,
            text: "Клавиатура удалена.",
            replyMarkup: new ReplyKeyboardRemove()
        );
    }

    private async Task HandleNonCommandMessageAsync(Message message)
    {
        if(string.IsNullOrWhiteSpace(message.Text) || message.From == null || string.IsNullOrWhiteSpace(message.From.Username))
            return;
            
        var isIncome = message.Text.StartsWith("+");
        var msgText = isIncome ? message.Text[1..] : message.Text;
        var isFloat = float.TryParse(msgText, NumberStyles.Any, CultureInfo.InvariantCulture, out var num);
        if (isFloat)
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
            var keyboard = GetInlineKeyboard(isIncome, dataId.ToString());
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"{savedData.Sign}{num} руб",
                replyMarkup: keyboard
            );
        }
        else
        {
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Я не понимаю, что вы от меня хотите."
            );
        }
    }

    private InlineKeyboardMarkup GetInlineKeyboard(bool isIncome, string savedDataId)
    {
        var categoryCollection = _categoryService.Get();
        var categoriesArrays = categoryCollection.ToArray().SplitArray(8);

        var inlineKeyboard = new InlineKeyboardMarkup(categoriesArrays.Select(arr =>
            arr.Select(x =>
            {
                var callback = new CallbackInfoDto { Id = savedDataId, Ctg = x.Name };
                return InlineKeyboardButton.WithCallbackData(x.Icon, callback.ToString());
            }).ToArray()
        ).ToArray());
        return inlineKeyboard;
    }

    private async Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        await _botClient.SendChatActionAsync(callbackQuery.Message.Chat.Id, ChatAction.Typing);
        var callbackData = JsonConvert.DeserializeObject<CallbackInfoDto>(callbackQuery.Data);
        var categoryCollection = _categoryService.Get();
        var category = categoryCollection.First(x => x.Name == callbackData.Ctg);
        var msg = $"{callbackQuery.Message.Text}, в категории \"{category.HumanName}\" {category.Icon}";

        var data = _moneyTransaction.Get(callbackData.Id);
        if (data == null)
        {
            _logger.LogError("Отсуствуют данные для обновдления. {msq}", category);
            return;
        }

        data.CategoryHumanName = category.HumanName;
        data.CategoryName = category.Name;
        data.CategoryIcon = category.Icon;
        _moneyTransaction.Update(data);

        await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id, $"Категория выбрана");
        await _botClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId,
            msg,
            replyMarkup: new InlineKeyboardMarkup(Array.Empty<InlineKeyboardButton>()));
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    private Task HandleErrorAsync(Exception exception)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException =>
                $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", errorMessage);
        return Task.CompletedTask;
    }
}