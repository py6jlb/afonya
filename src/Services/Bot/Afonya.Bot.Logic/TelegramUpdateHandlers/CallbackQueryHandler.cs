using Afonya.Bot.Domain.Entities;
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
    private readonly IBotKeyboardService _botKeyboard;
    private readonly ICallbackRepository _callbackRepository;

    public CallbackQueryHandler(ILogger<CallbackQueryHandler> logger, ITelegramBotClient botClient, 
        IMoneyTransactionRepository moneyTransaction, IBotKeyboardService botKeyboard, ICallbackRepository callbackRepository) : base(logger, botClient)
    {
        _moneyTransaction = moneyTransaction;
        _botKeyboard = botKeyboard;
        _callbackRepository = callbackRepository;
    }

    public override async Task HandleAsync(Update update, long chatId, CancellationToken ct = default)
    {
        Logger.LogInformation("Получено обновления типа: CallbackQuery");
        var callbackQuery = update.CallbackQuery!;
        if (string.IsNullOrWhiteSpace(callbackQuery.Data)) return;

        var callback = _callbackRepository.Get(callbackQuery.Data);
        if (callback == null) return;

        switch (callback.Command)
        {
            case CallbackCommand.SetCategory:
                await SetCategory(callbackQuery, callback, ct);
                break;
            case CallbackCommand.DeleteRequest:
                await DeleteEntryConfirm(callbackQuery, callback, ct);
                break;
            case CallbackCommand.Delete:
                await DeleteEntry(callbackQuery, callback, ct);
                break;
        }

        var res = _callbackRepository.Delete(callbackQuery.Data);
        if (callback.GroupId.HasValue)
        {
            var group = _callbackRepository.GetGroup(callback.GroupId.Value);
            foreach (var groupCallback in group ?? Array.Empty<Callback>())
            {
                _callbackRepository.Delete(groupCallback.Id.ToString());
            }
        }
    }

    private async Task SetCategory(CallbackQuery? callbackQuery, Callback callback, CancellationToken ct)
    {
        var callbackData = JsonConvert.DeserializeObject<SetCategoryCallbackData>(callback.JsonData);
        var msg = $"{callbackQuery.Message.Text}, в категории \"{callbackData.Category.HumanName}\" {callbackData.Category.Icon}";

        var data = _moneyTransaction.Get(callbackData.DataId);
        if (data == null)
        {
            Logger.LogError("Отсутствуют данные для обновления. {msq}", callbackData.Category);
            return;
        }

        data.CategoryHumanName = callbackData.Category.HumanName;
        data.CategoryName = callbackData.Category.Name;
        data.CategoryIcon = callbackData.Category.Icon;
        _moneyTransaction.Update(data);

        await BotClient.AnswerCallbackQueryAsync(callbackQuery.Id, $"Категория выбрана", cancellationToken: ct);
        var deleteKeyboard = _botKeyboard.GetDeleteKeyboard(callbackData.DataId, msg);
        await BotClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId, msg, replyMarkup: deleteKeyboard, cancellationToken: ct);
    }

    private async Task DeleteEntry(CallbackQuery? callbackQuery, Callback callback, CancellationToken ct)
    {
        var callbackData = JsonConvert.DeserializeObject<DeleteRequestCallbackData>(callback.JsonData);
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

    private async Task DeleteEntryConfirm(CallbackQuery? callbackQuery, Callback callback, CancellationToken ct)
    {
        var callbackData = JsonConvert.DeserializeObject<DeleteRequestCallbackData>(callback.JsonData);
        var confirmKeyboard = _botKeyboard.GetDeleteConfirmKeyboard(callbackData);
        var msg = $"Вы действительно хотите удалить запись: {callbackQuery.Message.Text}";
        await BotClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId, msg, replyMarkup: confirmKeyboard, cancellationToken: ct);
    }
}