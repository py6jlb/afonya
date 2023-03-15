using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Logic.Delegates;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.Logic.Commands.Bot.NewTelegramEvent;

public class NewTelegramEventCommandHandler : IRequestHandler<NewTelegramEventCommand, bool>
{
    private readonly ILogger<NewTelegramEventCommandHandler> _logger;
    private readonly IUserRepository _userRepository;
    private readonly TelegramHandlerResolver _resolver;

    public NewTelegramEventCommandHandler(IUserRepository userRepository, 
        ILogger<NewTelegramEventCommandHandler> logger, TelegramHandlerResolver resolver)
    {
        _userRepository = userRepository;
        _logger = logger;
        _resolver = resolver;
    }

    public async Task<bool> Handle(NewTelegramEventCommand request, CancellationToken cancellationToken)
    {
        var (allowed, chatId) = AllowedUser(request.Update);
        var handler = _resolver(request.Update.Type);
        
        if (!chatId.HasValue)
        {
            _logger.LogWarning("Ошибка обработки обновления. Отсутствует ChatId");
            return false;
        }

        if (!allowed)
        {
            await handler.NotAllowed(chatId.Value, cancellationToken);
            return false;
        }

        try
        {
            await handler.HandleAsync(request.Update, chatId.Value, cancellationToken);
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

    
}