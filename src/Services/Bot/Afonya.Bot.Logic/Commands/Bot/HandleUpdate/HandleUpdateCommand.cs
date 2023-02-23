using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Commands.Bot.HandleUpdate;

public class HandleUpdateCommand : IRequest<bool>
{
    public Update Update { get; set; }
}