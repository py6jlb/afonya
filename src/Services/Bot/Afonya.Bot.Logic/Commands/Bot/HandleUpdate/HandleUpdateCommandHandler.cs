using Afonya.Bot.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Commands.Bot.HandleUpdate;

public class HandleUpdateCommandHandler : IRequestHandler<HandleUpdateCommand, bool>
{
    private readonly ILogger<HandleUpdateCommandHandler> _logger;
    private readonly IUserService _userService;
    private readonly ITelegramUpdateService _updateService;

    public HandleUpdateCommandHandler(IUserService userService, ITelegramUpdateService updateService, ILogger<HandleUpdateCommandHandler> logger)
    {
        _userService = userService;
        _updateService = updateService;
        _logger = logger;
    }

    public async Task<bool> Handle(HandleUpdateCommand request, CancellationToken cancellationToken)
    {
        var allowed = AllowedUser(request.Update);
        if (!allowed) return false;
        
        var handler = request.Update switch
        {
            { Message: { } message }                       => _updateService.BotOnMessageAsync(message, cancellationToken),
            { EditedMessage: { } message }                 => _updateService.BotOnMessageAsync(message, cancellationToken),
            { CallbackQuery: { } callbackQuery }           => _updateService.BotOnCallbackQueryAsync(callbackQuery, cancellationToken),
            //{ InlineQuery: { } inlineQuery }               => _updateService.BotOnInlineQueryReceived(inlineQuery, cancellationToken),
            //{ ChosenInlineResult: { } chosenInlineResult } => _updateService.BotOnChosenInlineResultReceived(chosenInlineResult, cancellationToken),
            _                                              => _updateService.UnknownUpdateHandler(request.Update)
        };

        try
        {
            await handler;
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

    private bool AllowedUser(Update? update)
    {
        if(update == null) return false;
        var from = update switch
        {
            { Message: { } message }                       => message?.From?.Username,
            { EditedMessage: { } message }                 => message?.From?.Username,
            { CallbackQuery: { } callbackQuery }           => callbackQuery?.From?.Username,
            { InlineQuery: { } inlineQuery }               => inlineQuery?.From?.Username,
            { ChosenInlineResult: { } chosenInlineResult } => chosenInlineResult?.From?.Username,
            _                                              => null
        };

        return !string.IsNullOrWhiteSpace(from) && _userService.GetByName(from) != null;
    }
}