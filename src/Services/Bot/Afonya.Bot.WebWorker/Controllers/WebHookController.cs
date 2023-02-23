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
    //[ValidateTelegramBot]
    public async Task<IActionResult> Post([FromBody] Update update, CancellationToken cancellationToken)
    {
        var from = GetUserLogin(update);
        if (string.IsNullOrWhiteSpace(from) || _userService.GetByName(from) == null)
        {
            return Ok();
        }
        await _handleUpdateService.HandleUpdateAsync(update, cancellationToken);
        return Ok();
    }

    private static string? GetUserLogin(Update? update)
    {
        if(update == null) return null;
        return update switch
        {
            { Message: { } message }                       => update?.Message?.From?.Username,
            { EditedMessage: { } message }                 => update?.EditedMessage?.From?.Username,
            { CallbackQuery: { } callbackQuery }           => update?.CallbackQuery?.From?.Username,
            { InlineQuery: { } inlineQuery }               => update?.InlineQuery?.From?.Username,
            { ChosenInlineResult: { } chosenInlineResult } => update?.ChosenInlineResult?.From?.Username,
            _                                              => null
        };
    }
}