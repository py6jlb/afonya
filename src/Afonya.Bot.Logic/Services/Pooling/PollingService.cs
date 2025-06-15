using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.Logic.Services.Pooling;

public class PollingService
{
    private readonly ILogger _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IUpdateHandler _updateHandler;
    private CancellationTokenSource _cancelTokenSource;
    private bool _running = false;

    public PollingService(ILogger<PollingService> logger, ITelegramBotClient botClient, IUpdateHandler updateHandler)
    {
        _logger = logger;
        _botClient = botClient;
        _updateHandler = updateHandler;
        _cancelTokenSource = new CancellationTokenSource();
    }

    public async Task Start()
    {
        try
        {
            if (_running) return;
            _cancelTokenSource = new CancellationTokenSource();
            var stoppingToken = _cancelTokenSource.Token;
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                DropPendingUpdates = true,
            };

            var me = await _botClient.GetMe(stoppingToken);
            _logger.LogInformation("Начато получение событий для бота {BotName}", me.Username ?? "Afonya");
            await _botClient.DeleteWebhook(cancellationToken: stoppingToken);
            var task = _botClient.ReceiveAsync(updateHandler: _updateHandler,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken);
            SetState(true);
        }
        catch (Exception ex)
        {
            _logger.LogError("Polling failed with exception: {Exception}", ex);
            SetState(false);
        }
    }

    public Task Stop()
    {
        _cancelTokenSource.Cancel();
        SetState(false);
        return Task.CompletedTask;
    }

    public Task<bool> IsRunning()
    {
        return Task.FromResult(_running);
    }

    private void SetState(bool state)
    {
        if (!state)
        {
            _cancelTokenSource.Dispose();
        }
        object locker = new();
        lock (locker)
        {
            _running = state;
        }
    }
}