using Afonya.Bot.Domain.Exceptions;
using Afonya.Bot.Logic.Delegates;
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
    private readonly CommandBuilderResolver _resolver;

    public UpdateHandler(ILogger<UpdateHandler> logger, IMediator mediator, CommandBuilderResolver resolver)
    {
        _logger = logger;
        _mediator = mediator;
        _resolver = resolver;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        try
        {
            var builder = _resolver(update.Type);
            var command = builder.FromUpdate(update);
            await _mediator.Send(command, cancellationToken);
        }
        catch (AfonyaForbiddenException e)
        {
            _logger.LogWarning(e, "Неизвестный пользователь");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка обработки обновления");
            throw;
        }
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