using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Dto.CallbackData;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Interfaces.Services;

public interface IBotKeyboardService
{
    InlineKeyboardMarkup GetCategoryKeyboard(bool isIncome, string savedDataId);
    InlineKeyboardMarkup GetDeleteKeyboard(string savedDataId, string originalText);
    InlineKeyboardMarkup GetDeleteConfirmKeyboard(DeleteRequestCallbackData data);
}