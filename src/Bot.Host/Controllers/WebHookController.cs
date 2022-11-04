using Bot.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Bot.Host.Controllers;

public class WebHookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] IHandleUpdateService handleUpdateService, [FromBody] Update update)
    {
        await handleUpdateService.HandleUpdate(update);
        return Ok();
    }
}