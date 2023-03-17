using Afonya.Bot.Domain.Exceptions;
using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Logic.Bot.Commands.Unknown;
using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Bot.CommandBuilders;

public abstract class BaseCommandBuilder : ICommandBuilder
{
    public IBaseRequest FromUpdate(Update? update)
    {
        if (update == null)
            throw new AfonyaErrorException("Обновление не может быть пустым");

        return BuildCommand(update);
    }

    protected virtual IBaseRequest BuildCommand(Update update)
    {
        return BuildUnknownUpdateCommand(update);
    }

    protected (string from, long chatId) GetFrom(Update? update)
    {
        

        var (from, chatId) = update switch
        {
            { Message: { } message } => (message.From?.Username, message.Chat.Id),
            { EditedMessage: { } message } => (message.From?.Username, message.Chat.Id),
            { CallbackQuery: { } callbackQuery } => (callbackQuery.From.Username, callbackQuery.Message?.Chat.Id),
            _ => throw new AfonyaErrorException("Неизвестный тип обновления")
        };

        if (string.IsNullOrWhiteSpace(from) || !chatId.HasValue)
            throw new AfonyaErrorException("Отсутствует ID чата или имя пользователя");

        return (from, chatId.Value);
    }

    protected virtual IBaseRequest BuildUnknownUpdateCommand(Update update)
    {
        var (from, chatId) = GetFrom(update);
        return new UnknownCommand
        {
            ChatId = chatId,
            From = from
        };
    }
}