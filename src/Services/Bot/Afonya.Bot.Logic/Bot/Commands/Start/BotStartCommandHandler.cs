using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.Bot.Commands.Start;

public class BotStartCommandHandler : IRequestHandler<BotStartCommand, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly string? _help;

    public BotStartCommandHandler(ITelegramBotClient botClient, IConfiguration configuration)
    {
        _botClient = botClient;
        _help = configuration["Messages:Help"];
    }

    public async Task<bool> Handle(BotStartCommand request, CancellationToken cancellationToken)
    {
        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[] { new KeyboardButton("/Помощь") }) { ResizeKeyboard = true };
        await _botClient.SendMessage(chatId: request.ChatId, text: "Привет!", replyMarkup: replyKeyboardMarkup, cancellationToken: cancellationToken);
        await _botClient.SendMessage(chatId: request.ChatId, text: _help ?? "", cancellationToken: cancellationToken);
        return true;
    }
}