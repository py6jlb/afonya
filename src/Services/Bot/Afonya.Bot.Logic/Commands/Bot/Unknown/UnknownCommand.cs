using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.Unknown;

public class UnknownCommand : IRequest<bool>
{
    public long ChatId { get; set; }
}