using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.Start;

public class BotStartCommand : IRequest<bool>
{
    public long ChatId { get; set; }
}