using MediatR;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.Bot.Commands.RemoveKeyboard;

public class BotRemoveKeyboardCommandHandler : IRequestHandler<BotRemoveKeyboardCommand, bool>
{
    private readonly ITelegramBotClient _botClient;

    public BotRemoveKeyboardCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task<bool> Handle(BotRemoveKeyboardCommand request, CancellationToken cancellationToken)
    {
        await _botClient.SendTextMessageAsync(
            chatId: request.ChatId,
            text: "Клавиатура удалена.",
            replyMarkup: new ReplyKeyboardRemove(), cancellationToken: cancellationToken);
        return true;
    }
}