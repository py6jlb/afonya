using System;

namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public class YearKeyboardRequestCallbackData
{
    public int OriginalMessageId { get; set; }
    public int Year { get; set; }
}
