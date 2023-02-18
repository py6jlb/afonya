using Afonya.Bot.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace Afonya.Bot.WebWorker.Controllers;

[AllowAnonymous]
public class WebHookController : ControllerBase
{
    private readonly IHandleUpdateService _handleUpdateService;
    private readonly IUserService _userService;
    
    public WebHookController(IHandleUpdateService handleUpdateService, IUserService userService)
    {
        _handleUpdateService = handleUpdateService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        var from = update.Message?.From?.Username;
        if (string.IsNullOrWhiteSpace(from) || _userService.Get(from) == null)
        {
            return Ok();
        }
        await _handleUpdateService.HandleUpdate(update);
        return Ok();
    }
}