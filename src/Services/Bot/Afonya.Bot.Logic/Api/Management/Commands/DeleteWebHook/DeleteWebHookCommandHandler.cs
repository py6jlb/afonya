using Afonya.Bot.Interfaces.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Api.Management.Commands.DeleteWebHook;

public class DeleteWebHookCommandHandler : IRequestHandler<DeleteWebHookCommand, bool>
{
    private readonly ILogger<DeleteWebHookCommandHandler> _logger;
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly BotConfiguration _botConfig;

    public DeleteWebHookCommandHandler(ITelegramBotClient telegramBotClient, IOptions<BotConfiguration> botConfig,
        ILogger<DeleteWebHookCommandHandler> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
        _botConfig = botConfig.Value;
    }

    public async Task<bool> Handle(DeleteWebHookCommand request, CancellationToken cancellationToken)
    {
        if (_botConfig.UsePooling) return true;
        _logger.LogInformation("Delete webHook");
        await _telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        return true;
    }
}