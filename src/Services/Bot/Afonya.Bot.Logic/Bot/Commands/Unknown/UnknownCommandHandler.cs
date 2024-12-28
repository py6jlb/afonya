using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Bot.Commands.Unknown;

public class UnknownCommandHandler : IRequestHandler<UnknownCommand, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly string? _unknown;

    public UnknownCommandHandler(ITelegramBotClient botClient, IConfiguration configuration)
    {
        _botClient = botClient;
        _unknown = configuration["Messages:Unknown"];
    }

    public async Task<bool> Handle(UnknownCommand request, CancellationToken cancellationToken)
    {
        await _botClient.SendMessage(chatId: request.ChatId, text: _unknown ?? "-", cancellationToken: cancellationToken);
        return true;
    }
}