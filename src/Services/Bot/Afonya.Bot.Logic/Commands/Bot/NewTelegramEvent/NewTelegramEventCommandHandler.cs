using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services.UpdateHandler;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Commands.Bot.NewTelegramEvent;

public class NewTelegramEventCommandHandler : IRequestHandler<NewTelegramEventCommand, bool>
{
    private readonly ILogger<NewTelegramEventCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly ITelegramUpdateHandlerFactory _handlerFactory;

    public NewTelegramEventCommandHandler(IUserRepository userRepository, ILogger<NewTelegramEventCommandHandler> logger, ITelegramUpdateHandlerFactory handlerFactory)
    {
        _userRepository = userRepository;
        _logger = logger;
        _handlerFactory = handlerFactory;
    }

    public async Task<bool> Handle(NewTelegramEventCommand request, CancellationToken cancellationToken)
    {
        var (allowed, chatId) = AllowedUser(request.Update);
        var handler = _handlerFactory.CreateHandler(request.Update);
        
        if (!chatId.HasValue)
        {
            _logger.LogWarning("Ошибка обработки обновления. отсутствует ChatId");
            return false;
        }

        if (!allowed)
        {
            await handler.NotAllowed(chatId.Value, cancellationToken);
            return false;
        }


        try
        {
            await handler.HandleAsync(request.Update, chatId.Value, cancellationToken );
        }
        catch (Exception exception)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException =>
                    $"Ошибка Telegram API :\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation("При обработке обновления возникла ошибка: {ErrorMessage}", errorMessage);
        }
        return true;
    }

    private (bool allowed, long? chatId) AllowedUser(Update? update)
    {
        if(update == null) return (false, null);

        var (from, chatId) = update switch
        {
            { Message: { } message }                       => (message?.From?.Username, message?.Chat.Id),
            { EditedMessage: { } message }                 => (message?.From?.Username, message?.Chat.Id),
            { CallbackQuery: { } callbackQuery }           => (callbackQuery?.From?.Username, callbackQuery?.Message?.Chat.Id),
            _                                              => (null, null)
        };

        return (!string.IsNullOrWhiteSpace(from) && _userRepository.GetByName(from) != null, chatId);
    }
}