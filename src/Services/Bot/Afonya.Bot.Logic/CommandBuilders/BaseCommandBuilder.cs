using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Logic.Commands.Bot.Unknown;
using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.CommandBuilders;

public abstract class BaseCommandBuilder : ICommandBuilder
{
    public IBaseRequest BuildUnknownUpdateCommand(Update update)
    {
        return new UnknownCommand
        {
            ChatId = update.Message.Chat.Id
        };
    }

    public virtual IBaseRequest FromUpdate(Update update)
    {
        return BuildUnknownUpdateCommand(update);
    }

    protected (string? from, long? chatId) GetFrom(Update? update)
    {
        if(update == null) return (null, null);

        return update switch
        {
            { Message: { } message }                       => (message?.From?.Username, message?.Chat.Id),
            { EditedMessage: { } message }                 => (message?.From?.Username, message?.Chat.Id),
            { CallbackQuery: { } callbackQuery }           => (callbackQuery?.From?.Username, callbackQuery?.Message?.Chat.Id),
            _                                              => (null, null)
        };
    }
}