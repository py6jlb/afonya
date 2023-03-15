using MediatR;

namespace Afonya.Bot.Logic.Queries.Bot.Help;

public class BotHelpQuery : IRequest<bool>
{
    public long ChatId { get; set; }
}