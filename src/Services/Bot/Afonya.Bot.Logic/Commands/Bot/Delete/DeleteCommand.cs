using Afonya.Bot.Interfaces.Dto.CallbackData;
using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.Delete;

public class DeleteCommand : IRequest<bool>
{
    public int MessageId { get; set; }
    public string MessageText { get; set; }
    public long ChatId { get; set; }
    public string CallbackQueryId { get; set; }
    public DeleteRequestCallbackData CallbackData { get; set; }
}