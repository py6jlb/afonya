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
            var callbackId = _callbackRepository.Create(new Callback
            {
                Command = CallbackCommand.SetCategory,
                GroupId = groupId,
                JsonData = JsonConvert.SerializeObject(new SetCategoryCallbackData
                    { DataId = savedDataId, Category = category })
            });

            var button = InlineKeyboardButton.WithCallbackData(category.Icon, callbackId.Id.ToString());
            buttons.Add(button);
        }

        var buttonRows = buttons.ToArray().SplitArray(7);
        var inlineKeyboard = new InlineKeyboardMarkup(buttonRows);
        return inlineKeyboard;
    }

    public InlineKeyboardMarkup GetDeleteKeyboard(string savedDataId, string originalText)
    {
        var callbackId = _callbackRepository.Create(new Callback
        {
            Command = CallbackCommand.DeleteRequest,
            JsonData = JsonConvert.SerializeObject(new DeleteRequestCallbackData { DataId = savedDataId, OriginalMessageText = originalText})
        });
        
        var inlineKeyboard = new InlineKeyboardMarkup(new[] {InlineKeyboardButton.WithCallbackData("Удалить", callbackId.Id.ToString())});
        return inlineKeyboard;
    }

    public InlineKeyboardMarkup GetDeleteConfirmKeyboard(DeleteRequestCallbackData data)
    {
        var groupId = Guid.NewGuid();

        var yesCallbackId =  _callbackRepository.Create(new Callback
        {
            Command = CallbackCommand.Delete,
            GroupId = groupId,
            JsonData = JsonConvert.SerializeObject(data with { Confirm = true })
        });

        var noCallbackId = _callbackRepository.Create(new Callback
        {
            Command = CallbackCommand.Delete,
            GroupId = groupId,
            JsonData = JsonConvert.SerializeObject(data with { Confirm = false })
        });
        
        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("Да", yesCallbackId.Id.ToString()),
            InlineKeyboardButton.WithCallbackData("Нет", noCallbackId.Id.ToString())
        };

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);
        return inlineKeyboard;
    }
}