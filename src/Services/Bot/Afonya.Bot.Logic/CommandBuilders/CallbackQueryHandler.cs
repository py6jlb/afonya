﻿using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Domain.Enums;
using Afonya.Bot.Interfaces.Dto.CallbackData;
using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Logic.Commands.Bot.Delete;
using Afonya.Bot.Logic.Commands.Bot.SetCategory;
using Afonya.Bot.Logic.Queries.Bot.ConfirmDelete;
using Common.Exceptions;
using MediatR;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.CommandBuilders;

public class CallbackQueryHandler : BaseCommandBuilder
{
    private readonly ICallbackRepository _callbackRepository;
    public CallbackQueryHandler(ICallbackRepository callbackRepository)
    {
        _callbackRepository = callbackRepository;
    }

    public override IBaseRequest FromUpdate(Update update)
    {
        if (string.IsNullOrWhiteSpace(update.CallbackQuery?.Data))
            throw new AfonyaErrorException($"Нет данных callback");
        
        var callback = _callbackRepository.Get(update.CallbackQuery.Data);
        if (callback == null) 
            throw new AfonyaErrorException($"Callback с id {update.CallbackQuery.Data} ненайден в БД.");

        var command = callback.Command switch
        {
            CallbackCommand.SetCategory => BuildSetCategoryCommand(update, callback),
            CallbackCommand.DeleteRequest => BuildConfirmDeleteQuery(update, callback),
            CallbackCommand.Delete => BuildDeleteCommand(update, callback),
            _ => null
        };

        if (command == null)
            throw new AfonyaErrorException("Неизвестный тип команды");

        var _ = _callbackRepository.Delete(update.CallbackQuery.Data);
        if (!callback.GroupId.HasValue) return command;

        var group = _callbackRepository.GetGroup(callback.GroupId.Value);
        foreach (var groupCallback in group ?? Array.Empty<Callback>())
        {
            _callbackRepository.Delete(groupCallback.Id.ToString());
        }
        
        return command;
    }

    private IBaseRequest BuildSetCategoryCommand(Update update, Callback callback)
    {
        var callbackData = JsonConvert.DeserializeObject<SetCategoryCallbackData>(callback.JsonData);
        return new SetCategoryCommand{ 
            MessageText  = update.CallbackQuery.Message.Text,
            CallbackData = callbackData,
            CallbackQueryId = update.CallbackQuery.Id,
            MessageId = update.CallbackQuery.Message.MessageId,
            ChatId = update.CallbackQuery.Message.Chat.Id
        };
    }

    private IBaseRequest BuildConfirmDeleteQuery(Update update, Callback callback)
    {
        var callbackData = JsonConvert.DeserializeObject<DeleteRequestCallbackData>(callback.JsonData);
        return new ConfirmDeleteQuery
        {
            CallbackData = callbackData,
            ChatId = update.CallbackQuery.Message.Chat.Id,
            MessageId = update.CallbackQuery.Message.MessageId,
            MessageText = update.CallbackQuery.Message.Text
        };
    }

    private IBaseRequest BuildDeleteCommand(Update update, Callback callback)
    {
        var callbackData = JsonConvert.DeserializeObject<DeleteRequestCallbackData>(callback.JsonData);
        return new DeleteCommand
        {
            MessageText  = update.CallbackQuery.Message.Text,
            CallbackData = callbackData,
            CallbackQueryId = update.CallbackQuery.Id,
            MessageId = update.CallbackQuery.Message.MessageId,
            ChatId = update.CallbackQuery.Message.Chat.Id
        };
    }
}