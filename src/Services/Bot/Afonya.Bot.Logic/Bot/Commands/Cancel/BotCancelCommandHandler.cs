﻿using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.Bot.Commands.Cancel;

public class BotCancelCommandCommandHandler : IRequestHandler<BotCancelCommand, bool>
{
    private readonly ITelegramBotClient _botClient;

    public BotCancelCommandCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task<bool> Handle(BotCancelCommand request, CancellationToken cancellationToken)
    {
        await _botClient.SendMessage(
            chatId: request.ChatId,
            text: "Клавиатура удалена.",
            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
        return true;
    }
}