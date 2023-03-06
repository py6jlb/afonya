using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Domain.Enums;
using Afonya.Bot.Interfaces.Dto.CallbackData;
using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services;
using Common.Extensions;
using Newtonsoft.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.Services;

public class BotKeyboardService : IBotKeyboardService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICallbackRepository _callbackRepository;

    public BotKeyboardService(ICategoryRepository categoryRepository, ICallbackRepository callbackRepository)
    {
        _categoryRepository = categoryRepository;
        _callbackRepository = callbackRepository;
    }

    public InlineKeyboardMarkup GetCategoryKeyboard(bool isIncome, string savedDataId)
    {
        var groupId = Guid.NewGuid();
        var categoryCollection = _categoryRepository.Get();
        var buttons = new List<InlineKeyboardButton>();
        foreach (var category in categoryCollection)
        {
            var data = new SetCategoryCallbackData
            {
                DataId = savedDataId,
                Category = category
            };
            var callback = new Callback(CallbackCommand.SetCategory, groupId, JsonConvert.SerializeObject(data));
            var callbackId = _callbackRepository.Create(callback);

            var button = InlineKeyboardButton.WithCallbackData(category.Icon, callbackId.Id.ToString());
            buttons.Add(button);
        }

        var buttonRows = buttons.ToArray().SplitArray(7);
        var inlineKeyboard = new InlineKeyboardMarkup(buttonRows);
        return inlineKeyboard;
    }

    public InlineKeyboardMarkup GetDeleteKeyboard(string savedDataId, string originalText)
    {
        var data = new DeleteRequestCallbackData { DataId = savedDataId, OriginalMessageText = originalText };
        var newCallback = new Callback( CallbackCommand.DeleteRequest, null, JsonConvert.SerializeObject(data));
        var callback = _callbackRepository.Create(newCallback);
        
        var inlineKeyboard = new InlineKeyboardMarkup(new[] {InlineKeyboardButton.WithCallbackData("Удалить", callback.Id.ToString())});
        return inlineKeyboard;
    }

    public InlineKeyboardMarkup GetDeleteConfirmKeyboard(DeleteRequestCallbackData data)
    {
        var groupId = Guid.NewGuid();
        
        var yesData = data with { Confirm = true };
        var newYesCallback = new Callback(CallbackCommand.Delete, groupId, JsonConvert.SerializeObject(yesData));
        var yesCallback =  _callbackRepository.Create(newYesCallback);

        var noData = data with { Confirm = false };
        var newNoCallback = new Callback(CallbackCommand.Delete, groupId, JsonConvert.SerializeObject(noData));
        var noCallback = _callbackRepository.Create(newNoCallback);
        
        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("Да", yesCallback.Id.ToString()),
            InlineKeyboardButton.WithCallbackData("Нет", noCallback.Id.ToString())
        };

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);
        return inlineKeyboard;
    }
}