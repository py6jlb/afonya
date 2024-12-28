using MediatR;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Api.Management.Queries.BotWebhookStatus;

public class BotWebhookStatusQueryHandler : IRequestHandler<BotWebhookStatusQuery, WebhookInfo>
{
    private readonly ILogger<BotWebhookStatusQueryHandler> _logger;
    private readonly ITelegramBotClient _telegramBotClient;

    public BotWebhookStatusQueryHandler(ITelegramBotClient telegramBotClient,  ILogger<BotWebhookStatusQueryHandler> logger)
    {
        _telegramBotClient = telegramBotClient;
        _logger = logger;
    }

    public async Task<WebhookInfo> Handle(BotWebhookStatusQuery request, CancellationToken cancellationToken)
    {
        var result = await _telegramBotClient.GetWebhookInfoAsync(cancellationToken: cancellationToken);
        return result;
    }
}