using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Commands.Bot.NewTelegramEvent;

public class NewTelegramEventCommand : IRequest<bool>
{
    public Update Update { get; set; }
}