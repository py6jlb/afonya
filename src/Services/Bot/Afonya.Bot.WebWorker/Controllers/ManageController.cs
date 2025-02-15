﻿using Afonya.Bot.Logic.Api.Management.Commands.DeleteWebHook;
using Afonya.Bot.Logic.Api.Management.Commands.SetWebHook;
using Afonya.Bot.Logic.Api.Management.Queries.BotWebhookStatus;
using Afonya.Bot.WebWorker.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Afonya.Bot.WebWorker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ManageController : ControllerBase
{
    private readonly IMediator _mediator;


    public ManageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BasicAuthAdmin]
    [HttpPost("status")]
    public async Task<WebhookInfo> StatusBot(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new BotWebhookStatusQuery(), cancellationToken);
        return result;
    }
    
    [BasicAuthAdmin]
    [HttpPost("start")]
    public async Task StartBot(CancellationToken cancellationToken)
    {
        await _mediator.Send(new SetWebHookCommand(), cancellationToken);
    }
    
    [BasicAuthAdmin]
    [HttpPost("stop")]
    public async Task StopBot(CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteWebHookCommand(), cancellationToken);
    }
}