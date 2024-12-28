using Afonya.Bot.Interfaces.Dto.CallbackData;

namespace Afonya.Bot.Logic.Bot.Commands.SetCategory;

public sealed record SetCategoryCommand : BaseBotCommand<bool>
{
    public int MessageId { get; set; }
    public string MessageText { get; set; }
    public string CallbackQueryId { get; set; }
    public SetCategoryCallbackData CallbackData { get; set; }
}