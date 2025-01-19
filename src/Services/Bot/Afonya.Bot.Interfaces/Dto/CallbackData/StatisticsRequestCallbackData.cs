using System;

namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public class StatisticsRequestCallbackData
{
    public int OriginalMessageId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}
