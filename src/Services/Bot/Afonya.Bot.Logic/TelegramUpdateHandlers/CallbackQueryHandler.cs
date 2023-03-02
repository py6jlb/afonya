using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Interfaces.Services.UpdateHandler;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.UpdateHandlers;

public class CallbackQueryHandler : BaseHandler
{
    private readonly IMoneyTransactionService _moneyTransaction;
    private readonly ICategoryService _categoryService;

    public CallbackQueryHandler(ILogger<CallbackQueryHandler> logger, ITelegramBotClient botClient, 
        IMoneyTransactionService moneyTransaction, ICategoryService categoryService) : base(logger, botClient)
    {
        _moneyTransaction = moneyTransaction;
        _categoryService = categoryService;
    }

    public override async Task HandleAsync(Update update, long chatId, CancellationToken ct = default)
    {
        Logger.LogInformation("Получено обновления типа: CallbackQuery");
        var callbackQuery = update.CallbackQuery!;

        var callbackData = JsonConvert.DeserializeObject<CallbackInfo>(callbackQuery.Data);
        var categoryCollection = _categoryService.Get();
        var category = categoryCollection.First(x => x.Name == callbackData.Ctg);
        var msg = $"{callbackQuery.Message.Text}, в категории \"{category.HumanName}\" {category.Icon}";

        var data = _moneyTransaction.Get(callbackData.Id);
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
        await BotClient.EditMessageTextAsync(callbackQuery.Message.Chat.Id,
            callbackQuery.Message.MessageId,
            msg,
            replyMarkup: new InlineKeyboardMarkup(Array.Empty<InlineKeyboardButton>()), cancellationToken: ct);
    }
}