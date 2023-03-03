using Afonya.Bot.Domain.Enums;
using Afonya.Bot.Interfaces.Dto.CallbackData;
using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.TelegramUpdateHandlers;

public class CallbackQueryHandler : BaseHandler
{
    private readonly IMoneyTransactionRepository _moneyTransaction;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBotKeyboardService _botKeyboard;

    public CallbackQueryHandler(ILogger<CallbackQueryHandler> logger, ITelegramBotClient botClient, 
        IMoneyTransactionRepository moneyTransaction, ICategoryRepository categoryRepository, IBotKeyboardService botKeyboard) : base(logger, botClient)
    {
        _moneyTransaction = moneyTransaction;
        _categoryRepository = categoryRepository;
        _botKeyboard = botKeyboard;
    }

    public override async Task HandleAsync(Update update, long chatId, CancellationToken ct = default)
    {
        Logger.LogInformation("Получено обновления типа: CallbackQuery");
        var callbackQuery = update.CallbackQuery!;
        var callbackData = JsonConvert.DeserializeObject<CallbackData>(callbackQuery.Data);

        switch (callbackData.Command)
        {
            case CallbackCommand.SetCategory:
                await SetCategory(callbackQuery, ct);
                break;
            case CallbackCommand.DeleteRequest:
                await DeleteEntryConfirm(callbackQuery, ct);
                break;
            case CallbackCommand.Delete:
                await DeleteEntry(callbackQuery, ct);
                break;
            default:
                break;
        }
    }


    private async Task SetCategory(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        var callbackData = JsonConvert.DeserializeObject<SetCategory>(callbackQuery.Data);
        var categoryCollection = _categoryRepository.Get();
        var category = categoryCollection.First(x => x.Name == callbackData.Ctg);
        var msg = $"{callbackQuery.Message.Text}, в категории \"{category.HumanName}\" {category.Icon}";

        var data = _moneyTransaction.Get(callbackData.DataId);
        if (data == null)
        {
            Logger.LogError("Отсутствуют данные для обновления. {msq}", category);
            return;
        }

        data.CategoryHumanName = category.HumanName;
        data.CategoryName = category.Name;
        data.CategoryIcon = category.Icon;
        _moneyTransaction.Update(data);

        await BotClient.AnswerCallbackQueryAsync(callbackQuery.Id, $"Категория выбрана", cancellationToken: ct);
        var deleteKeyboard = _botKeyboard.GetDeleteKeyboard(callbackData.DataId, callbackQuery.Message.Text);
        await BotClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId, msg, replyMarkup: deleteKeyboard, cancellationToken: ct);

    }

    private async Task DeleteEntry(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        var callbackData = JsonConvert.DeserializeObject<DeleteRequest>(callbackQuery.Data);
        if (callbackData.Confirm)
        {
            var res = _moneyTransaction.Delete(callbackData.DataId);
            if (!res) return;
            await BotClient.AnswerCallbackQueryAsync(callbackQuery.Id, $"Запись удалена", cancellationToken: ct);
            await BotClient.DeleteMessageAsync(callbackQuery.Message.Chat.Id, callbackQuery.Message.MessageId, cancellationToken: ct);
        }

        var deleteKeyboard = _botKeyboard.GetDeleteKeyboard(callbackData.DataId, callbackData.OriginalMessageText);
        await BotClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId, callbackData.OriginalMessageText, replyMarkup: deleteKeyboard, cancellationToken: ct);
    }

    private async Task DeleteEntryConfirm(CallbackQuery? callbackQuery, CancellationToken ct)
    {
        var callbackData = JsonConvert.DeserializeObject<DeleteRequest>(callbackQuery.Data);
        var confirmKeyboard = _botKeyboard.GetDeleteConfirmKeyboard(callbackData);
        var msg = $"Вы действительно хотите удалить запись: {callbackQuery.Message.Text}";
        await BotClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId, msg, replyMarkup: confirmKeyboard, cancellationToken: ct);
    }
}