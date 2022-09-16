using Bot.Services.Dto;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Bot.Services;

public class ConfigureWebHook : IHostedService
{
    private readonly ILogger<ConfigureWebHook> _logger;
    private readonly IServiceProvider _services;
    private readonly BotConfiguration _botConfig;
    private readonly bool _useReverseProxy;
    private readonly string _subdir;

    public ConfigureWebHook(
        ILogger<ConfigureWebHook> logger, 
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = serviceProvider;
        _botConfig = configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
        _ = bool.TryParse(configuration["USE_REVERSE_PROXY"], out _useReverseProxy);
        _subdir = configuration["SUBDIR_PATH"] ?? "";
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webHookAddress = _useReverseProxy ? 
            $"{_botConfig.HostAddress}{_subdir}/bot/{_botConfig.BotToken}" : 
            $"{_botConfig.HostAddress}/bot/{_botConfig.BotToken}";
        _logger.LogInformation("Установка webHook: {webHookAddress}", webHookAddress);
        await botClient.SetWebhookAsync(url: webHookAddress, 
            allowedUpdates: Array.Empty<UpdateType>(),
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        _logger.LogInformation("Удаление webHook");
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}