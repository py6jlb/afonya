using Afonya.Bot.Logic.Commands.Bot.HandleUpdate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Afonya.Bot.WebWorker.Controllers;

[AllowAnonymous]
public class WebHookController : ControllerBase
{
    private readonly IMediator _mediator;

    public WebHookController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    //[ValidateTelegramBot]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        await _mediator.Send(new HandleUpdateCommand{ Update = update }, cancellationToken);
        return Ok();
    }
}