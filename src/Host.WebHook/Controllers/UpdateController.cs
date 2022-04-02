using Microsoft.AspNetCore.Mvc;

namespace Host.WebHook.Controllers;

[ApiController]
[Route("[controller]")]
public class WebHookController : ControllerBase
{
    private readonly ILogger<WebHookController> _logger;

    public WebHookController(ILogger<WebHookController> logger)
    {
        _logger = logger;
    }
}