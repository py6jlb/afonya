using Bot.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Bot.Host.Controllers;

public class WebHookController : ControllerBase
{
    private readonly IHandleUpdateService _handleUpdateService;

    public WebHookController(IHandleUpdateService handleUpdateService)
    {
        _handleUpdateService = handleUpdateService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        await _handleUpdateService.HandleUpdate(update);
        return Ok();
    }
}