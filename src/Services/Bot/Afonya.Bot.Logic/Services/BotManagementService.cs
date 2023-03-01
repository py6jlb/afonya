using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Services;
using Common.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.Logic.Services;

public class BotManagementService : IBotManagementService
{
    private readonly ILogger<BotManagementService> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly BotConfiguration _botConfig;
    private readonly ReverseProxyConfig _proxyConfig;

    public BotManagementService(ITelegramBotClient telegramBotClient, IOptions<BotConfiguration> botConfig,
        IOptions<ReverseProxyConfig> proxyConfig, ILogger<BotManagementService> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
        _botConfig = botConfig.Value;
        _proxyConfig = proxyConfig.Value;
    }


    public async Task StartAsync(CancellationToken ct = default)
    {
        if (!_botConfig.UsePooling)
        {
            var webHookAddress = _proxyConfig?.UseReverseProxy ?? false ? 
                $"{_botConfig.HostAddress}{_proxyConfig?.SubDir ?? ""}/bot/{_botConfig.BotToken}" : 
                $"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
            _logger.LogInformation("Set webHook: {WebHookAddress}", $"{webHookAddress}/webhook");
            await _telegramBotClient.SetWebhookAsync(url: $"{webHookAddress}/webhook", 
                allowedUpdates: Array.Empty<UpdateType>(), cancellationToken: ct);
        }
    }

    public async Task StopAsync(CancellationToken ct = default)
    {
        if (!_botConfig.UsePooling)
        {
            _logger.LogInformation("Delete webHook");
            await _telegramBotClient.DeleteWebhookAsync(cancellationToken: ct);
        }
    }

    public async Task<WebhookInfo> StatusAsync(CancellationToken ct = default)
    {
        var result = await _telegramBotClient.GetWebhookInfoAsync(cancellationToken: ct);
        return result;
    }
}