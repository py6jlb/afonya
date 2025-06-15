using Afonya.Bot.Interfaces.Dto;
using Afonya.Api.Logic.Management.Commands.CreateUser;
using MediatR;
using Microsoft.Extensions.Options;

namespace Afonya.Web.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapBotController(this WebApplication app)
    {
        var opt = app.Services.GetRequiredService<IOptions<BotConfiguration>>().Value;
        if (!opt.RunPooling)
        {
            app.MapControllerRoute(
                name: "tgwebhook",
                pattern: $"bot/{opt.BotToken}/webhook",
                new { controller = "WebHook", action = "Post" });
        }

        return app;
    }
}