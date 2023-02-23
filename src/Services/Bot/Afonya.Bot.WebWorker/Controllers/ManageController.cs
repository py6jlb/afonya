using Afonya.Bot.Logic.Commands.Bot.BotStart;
using Afonya.Bot.Logic.Commands.Bot.BotStop;
using Afonya.Bot.Logic.Queries.BotWebhookStatus;
using Afonya.Bot.WebWorker.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;
using Telegram.Bot.Types;

namespace Afonya.Bot.WebWorker.Controllers;

[ApiController]
//[Authorize]
[BasicAuthAdmin]
public class ManageController : ControllerBase
{
    private readonly IMediator _mediator;


    public ManageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("bot/status")]
    public async Task<WebhookInfo> StatusBot(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new BotWebhookStatusQuery(), cancellationToken);
        return result;
    }
    
    [HttpPost("bot/start")]
    public async Task StartBot(CancellationToken cancellationToken)
    {
        await _mediator.Send(new BotStartCommand(), cancellationToken);
    }
    
    [HttpPost("bot/stop")]
    public async Task StopBot(CancellationToken cancellationToken)
    {
        await _mediator.Send(new BotStopCommand(), cancellationToken);
    }
}