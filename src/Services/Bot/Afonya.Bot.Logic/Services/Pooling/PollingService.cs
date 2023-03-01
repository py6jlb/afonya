using Afonya.Bot.Interfaces.Pooling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.Logic.Services.Pooling;

public class PollingService : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public PollingService(ILogger<PollingService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting polling service");
        await DoWork(stoppingToken);
    }

    private async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
                var updateHandlers = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();

                var receiverOptions = new ReceiverOptions()
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                    ThrowPendingUpdates = true,
                };

                var me = await botClient.GetMeAsync(stoppingToken);
                _logger.LogInformation("Начато получение событий для бота {BotName}", me.Username ?? "Afonya");
                await botClient.DeleteWebhookAsync(cancellationToken: stoppingToken);
                await botClient.ReceiveAsync(updateHandler: updateHandlers, receiverOptions: receiverOptions, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Polling failed with exception: {Exception}", ex);
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}