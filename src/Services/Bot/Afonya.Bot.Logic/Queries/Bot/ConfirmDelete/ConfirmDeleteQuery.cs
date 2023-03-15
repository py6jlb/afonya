using Afonya.Bot.Interfaces.Dto.CallbackData;
using MediatR;

namespace Afonya.Bot.Logic.Queries.Bot.ConfirmDelete;

public class ConfirmDeleteQuery : IRequest<bool>
{
    public int MessageId { get; set; }
    public string MessageText { get; set; }
    public long ChatId { get; set; }
    public DeleteRequestCallbackData CallbackData { get; set; }
}