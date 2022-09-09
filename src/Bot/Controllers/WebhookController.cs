using Host.WebHook.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Host.WebHook.Controllers;

public class WebhookController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
        [FromBody] Update update)
    {
        await handleUpdateService.EchoAsync(update);
        return Ok();
    }
}