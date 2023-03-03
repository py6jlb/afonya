using Afonya.Bot.Domain.Enums;
using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Dto.CallbackData;
using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services;
using Common.Extensions;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.Services;

public class BotKeyboardService : IBotKeyboardService
{
    private readonly ICategoryRepository _categoryRepository;

    public BotKeyboardService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public InlineKeyboardMarkup GetCategoryKeyboard(bool isIncome, string savedDataId)
    {
        var categoryCollection = _categoryRepository.Get();
        var categoriesArrays = categoryCollection.ToArray().SplitArray(7);

        var inlineKeyboard = new InlineKeyboardMarkup(categoriesArrays.Select(arr =>
            arr.Select(x =>
            {
                var callback = new SetCategory { DataId = savedDataId, Ctg = x.Name, Command = CallbackCommand.SetCategory};
                var button = InlineKeyboardButton.WithCallbackData(x.Icon, callback.ToString());
                return button;
            }).ToArray()
        ).ToArray());
        return inlineKeyboard;
    }

    public InlineKeyboardMarkup GetDeleteKeyboard(string savedDataId, string originalText)
    {
        var callback = new DeleteRequest() { DataId = savedDataId,  Command = CallbackCommand.DeleteRequest, OriginalMessageText = originalText};
        var button = InlineKeyboardButton.WithCallbackData("Удалить", callback.ToString());
        
        var buttons = new[] {button};
        var inlineKeyboard = new InlineKeyboardMarkup(buttons);
        return inlineKeyboard;
    }

    public InlineKeyboardMarkup GetDeleteConfirmKeyboard(DeleteRequest data)
    {
        var yesData = data with { Confirm = true, Command = CallbackCommand.Delete };
        var noData = data with { Confirm = false, Command = CallbackCommand.Delete };
        
        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("Да", yesData.ToString()),
            InlineKeyboardButton.WithCallbackData("Нет", noData.ToString())
        };

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);
        return inlineKeyboard;
    }
}