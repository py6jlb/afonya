using MediatR;

namespace Afonya.Bot.Logic.Bot;

public record BaseBotCommand<TResponse> : IRequest<TResponse>
{
    public string From { get; set; }
    public long ChatId { get; set; }
}