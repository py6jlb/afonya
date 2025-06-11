using Afonya.Bot.Logic.Delegates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Afonya.Bot.WebWorker.Controllers;

[AllowAnonymous]
public class WebHookController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CommandBuilderResolver _resolver;

    public WebHookController(IMediator mediator, CommandBuilderResolver resolver)
    {
        _mediator = mediator;
        _resolver = resolver;
    }

    [HttpPost]
    //[ValidateTelegramBot]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        var builder = _resolver(update.Type);
        var command = builder.FromUpdate(update);
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }
}