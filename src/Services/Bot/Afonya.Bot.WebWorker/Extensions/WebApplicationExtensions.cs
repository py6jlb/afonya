using Afonya.Bot.Interfaces.Dto;
using Microsoft.Extensions.Options;

namespace Afonya.Bot.WebWorker.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapBotController(this WebApplication app)
    {
        var opt = app.Services.GetRequiredService<IOptions<BotConfiguration>>().Value;
        if (!opt.UsePooling)
        {
            app.MapControllerRoute(
                name: "tgwebhook",
                pattern: $"bot/{opt.BotToken}/webhook", 
                new { controller = "WebHook", action = "Post" });
        }
        
        return app;
    }
}