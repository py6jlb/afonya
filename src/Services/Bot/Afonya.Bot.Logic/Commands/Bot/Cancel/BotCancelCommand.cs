using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.Cancel;

public class BotCancelCommand : IRequest<bool>
{
    public long ChatId { get; set; }
}