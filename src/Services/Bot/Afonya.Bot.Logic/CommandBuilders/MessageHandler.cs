using Afonya.Bot.Logic.Commands.Bot.Cancel;
using Afonya.Bot.Logic.Commands.Bot.NewMessage;
using Afonya.Bot.Logic.Commands.Bot.RemoveKeyboard;
using Afonya.Bot.Logic.Commands.Bot.Start;
using Afonya.Bot.Logic.Queries.Bot.Help;
using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.CommandBuilders;

public class MessageHandler : BaseCommandBuilder
{
    public override IBaseRequest FromUpdate(Update update)
    {
        var command = (update.Message?.Text?.Split(' ').FirstOrDefault()?.ToLower()) switch
        {
            "/начать" or "/start"  => BuildStartCommand(update),
            "/закрыть" or "/cancel" => BuildCloseCommand(update),
            "/помощь" or "/help" => BuildHelpCommand(update),
            "/kbrm" => BuildRemoveKeyboardCommand(update),
            _ => BuildNewMessageCommand(update)
        };

        return command;
    }

    private IBaseRequest BuildStartCommand(Update update)
    {
        return new BotStartCommand{ChatId = update.Message.Chat.Id};
    }

    private IBaseRequest BuildCloseCommand(Update update)
    {
        return new BotCancelCommand{ChatId = update.Message.Chat.Id};
    }

    private IBaseRequest BuildHelpCommand(Update update)
    {
        return new BotHelpQuery{ChatId = update.Message.Chat.Id};
    }

    private IBaseRequest BuildRemoveKeyboardCommand(Update update)
    {
        return new BotRemoveKeyboardCommand{ChatId = update.Message.Chat.Id};
    }
    
    private IBaseRequest BuildNewMessageCommand(Update update)
    {
        return new NewMessageCommand
        {
            ChatId = update.Message.Chat.Id, 
            MessageDate = update.Message.Date, 
            MessageId = update.Message.MessageId, 
            MessageText = update.Message.Text, 
            UserName = update.Message.From.Username
        };
    }
}