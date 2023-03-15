using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.NewMessage;

public class NewMessageCommand : IRequest<bool>
{
    public long ChatId { get; set; }
    public int MessageId { get; set; }
    public string MessageText { get; set; }
    public DateTime MessageDate { get; set; }
    public string UserName { get; set; }
}