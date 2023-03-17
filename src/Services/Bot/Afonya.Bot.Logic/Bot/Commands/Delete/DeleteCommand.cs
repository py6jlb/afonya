using Afonya.Bot.Interfaces.Dto.CallbackData;

namespace Afonya.Bot.Logic.Bot.Commands.Delete;

public sealed record DeleteCommand : BaseBotCommand<bool>
{
    public int MessageId { get; set; }
    public string MessageText { get; set; }
    public string CallbackQueryId { get; set; }
    public DeleteRequestCallbackData CallbackData { get; set; }
}