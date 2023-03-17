﻿using Afonya.Bot.Logic.Bot.Commands.Cancel;
using Afonya.Bot.Logic.Bot.Commands.NewMessage;
using Afonya.Bot.Logic.Bot.Commands.RemoveKeyboard;
using Afonya.Bot.Logic.Bot.Commands.Start;
using Afonya.Bot.Logic.Bot.Queries.Help;
using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Bot.CommandBuilders;

public class MessageHandler : BaseCommandBuilder
{
    protected override IBaseRequest BuildCommand(Update update)
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
        var (from, chatId) = GetFrom(update);
        return new BotStartCommand
        {
            ChatId = chatId,
            From = from
        };
    }

    private IBaseRequest BuildCloseCommand(Update update)
    {
        var (from, chatId) = GetFrom(update);
        return new BotCancelCommand{ChatId = update.Message.Chat.Id};
    }

    private IBaseRequest BuildHelpCommand(Update update)
    {
        var (from, chatId) = GetFrom(update);
        return new BotHelpQuery
        {
            ChatId = chatId,
            From = from
        };
    }

    private IBaseRequest BuildRemoveKeyboardCommand(Update update)
    {
        var (from, chatId) = GetFrom(update);
        return new BotRemoveKeyboardCommand
        {
            ChatId = chatId,
            From = from
        };
    }
    
    private IBaseRequest BuildNewMessageCommand(Update update)
    {
        var (from, chatId) = GetFrom(update);
        return new NewMessageCommand
        {
            MessageDate = update.Message.Date, 
            MessageId = update.Message.MessageId, 
            MessageText = update.Message.Text, 
            ChatId = chatId,
            From = from
        };
    }
}