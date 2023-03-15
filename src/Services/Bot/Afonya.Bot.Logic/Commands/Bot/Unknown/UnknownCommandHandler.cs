using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Commands.Bot.Unknown;

public class UnknownCommandHandler : IRequestHandler<UnknownCommand, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly string? _unknown;

    public UnknownCommandHandler(ITelegramBotClient botClient, IConfiguration configuration)
    {
        _botClient = botClient;
        _unknown = configuration["Messages:Unknown"];
    }

    public Task<bool> Handle(UnknownCommand request, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(chatId: request.ChatId, text: _unknown ?? "-", cancellationToken: cancellationToken);
        return true;
    }
}