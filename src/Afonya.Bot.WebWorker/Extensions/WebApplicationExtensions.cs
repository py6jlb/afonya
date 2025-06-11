using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Logic.Api.Management.Commands.CreateUser;
using MediatR;
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

    public static async Task<WebApplication> InitUsers(this WebApplication app)
    {
        var configuration = app.Services.GetRequiredService<IConfiguration>();
        var users = configuration.GetSection("Users").Get<UserConfig[]>() ?? [];
        var mediator = app.Services.GetRequiredService<IMediator>();
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        foreach (var userConfig in users)
        {
            try
            {
                await mediator.Send(new CreateUserCommand
                {
                    Login = userConfig.Username,
                    Password = userConfig.Password,
                    IsAdmin = userConfig.IsAdmin
                });
                logger.LogInformation("Пользователь создан: {username}", userConfig.Username);
            }
            catch (Exception error)
            {
                logger.LogInformation("Пользователь не создан: {message}", error.Message);
            }
        }


        return app;
    }
}