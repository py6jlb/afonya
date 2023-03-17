using Afonya.Bot.Interfaces.Dto;
using Common.Options;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.Logic.Api.Management.Commands.SetWebHook;

public class SetWebHookCommandHandler : IRequestHandler<SetWebHookCommand, bool>
{
    private readonly ILogger<SetWebHookCommandHandler> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly BotConfiguration _botConfig;
    private readonly ReverseProxyConfig _proxyConfig;

    public SetWebHookCommandHandler(ITelegramBotClient telegramBotClient, IOptions<BotConfiguration> botConfig,
        IOptions<ReverseProxyConfig> proxyConfig, ILogger<SetWebHookCommandHandler> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
        _botConfig = botConfig.Value;
        _proxyConfig = proxyConfig.Value;
    }

    public async Task<bool> Handle(SetWebHookCommand request, CancellationToken cancellationToken)
    {
        if (_botConfig.UsePooling) return true;

        var webHookAddress = _proxyConfig?.UseReverseProxy ?? false ? 
            $"{_botConfig.HostAddress}{_proxyConfig?.SubDir ?? ""}/bot/{_botConfig.BotToken}" : 
            $"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
        _logger.LogInformation("Set webHook: {WebHookAddress}", $"{webHookAddress}/webhook");
        await _telegramBotClient.SetWebhookAsync(url: $"{webHookAddress}/webhook", 
            allowedUpdates: Array.Empty<UpdateType>(), cancellationToken: cancellationToken);

        return true;
    }
}