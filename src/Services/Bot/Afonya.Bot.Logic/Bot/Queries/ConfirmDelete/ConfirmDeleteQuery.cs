using Afonya.Bot.Interfaces.Dto.CallbackData;

namespace Afonya.Bot.Logic.Bot.Queries.ConfirmDelete;

public sealed record ConfirmDeleteQuery : BaseBotCommand<bool>
{
    public int MessageId { get; set; }
    public string MessageText { get; set; }
    public DeleteRequestCallbackData CallbackData { get; set; }
}