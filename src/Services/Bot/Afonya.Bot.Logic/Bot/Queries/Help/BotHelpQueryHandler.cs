﻿using MediatR;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Bot.Queries.Help;

public class BotHelpQueryHandler : IRequestHandler<BotHelpQuery, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly string? _help;

    public BotHelpQueryHandler(ITelegramBotClient botClient, IConfiguration configuration)
    {
        _botClient = botClient;
        _help = configuration["Messages:Help"];
    }

    public async Task<bool> Handle(BotHelpQuery request, CancellationToken cancellationToken)
    {
        await _botClient.SendMessage(chatId: request.ChatId, text: _help ?? "", cancellationToken: cancellationToken);
        return true;
    }
}