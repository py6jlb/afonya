using Afonya.Bot.Interfaces.Services;
using MediatR;
using Telegram.Bot.Types;

namespace Afonya.Bot.Logic.Queries.BotWebhookStatus;

public class BotWebhookStatusQueryHandler : IRequestHandler<BotWebhookStatusQuery, WebhookInfo>
{
    private readonly IBotManagementService _botManagementService;

    public BotWebhookStatusQueryHandler(IBotManagementService botManagementService)
    {
        _botManagementService = botManagementService;
    }

    public async Task<WebhookInfo> Handle(BotWebhookStatusQuery request, CancellationToken cancellationToken)
    {
        var result = await _botManagementService.StatusAsync(cancellationToken);
        return result;
    }
}