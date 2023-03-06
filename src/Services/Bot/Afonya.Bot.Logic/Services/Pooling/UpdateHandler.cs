using Afonya.Bot.Logic.Commands.Bot.NewTelegramEvent;
using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Services.Pooling;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly IMediator _mediator;

    public UpdateHandler(ILogger<UpdateHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        await _mediator.Send(new NewTelegramEventCommand {Update = update}, cancellationToken);
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("Обработка ошибки: {ErrorMessage}", errorMessage);
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}