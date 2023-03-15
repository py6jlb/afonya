using MediatR;

namespace Afonya.Bot.Logic.Commands.Bot.RemoveKeyboard;

public class BotRemoveKeyboardCommand : IRequest<bool>
{
    public long ChatId { get; set; }
}