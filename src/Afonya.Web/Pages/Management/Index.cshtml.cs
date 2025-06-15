using Afonya.Bot.Logic.Services.Pooling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Afonya.Web.Pages.Management;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly PollingService _poolingService;

    public IndexModel(ILogger<IndexModel> logger, PollingService poolingService)
    {
        _logger = logger;
        _poolingService = poolingService;
    }

    public bool BotIsRunning { get; set; } = true;

    public async Task OnGetAsync()
    {
        BotIsRunning = await _poolingService.IsRunning();
    }

    public async Task<IActionResult> OnPostBotStart()
    {
        await _poolingService.Start();
        return RedirectToPage("/Management/Index");
    }

    public async Task<IActionResult> OnPostBotStop()
    {
        await _poolingService.Stop();
        return RedirectToPage("/Management/Index");
    }
}
