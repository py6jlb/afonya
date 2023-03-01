using System.Reflection;
using Afonya.Bot.Infrastructure.Contexts;
using Afonya.Bot.Interfaces;
using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Interfaces.Services.UpdateHandler;
using Afonya.Bot.Logic.Commands.Bot.HandleUpdate;
using Afonya.Bot.Logic.Services;
using Afonya.Bot.Logic.UpdateHandlers;
using Common.Exceptions;
using Common.Options;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Afonya.Bot.WebWorker.Extensions;

public static class StartupExtensions
{
    public static void AddErrorHandling(this IServiceCollection services, IHostEnvironment env)
    {
        services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => !env.IsDevelopment();

                options.Map<AfonyaErrorException>(exception => new ProblemDetails
                {
                    Type = nameof(AfonyaErrorException),
                    Title = "Ошибка",
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError,
                });
                
                options.Map<Exception>(exception => new ProblemDetails
                {
                    Type = "AfonyaException",
                    Title = "Ошибка",
                    Detail = exception.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        );
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<BotConfiguration>(config.GetSection("BotConfiguration"));
        services.Configure<AdminUser>(config.GetSection("AdminUser"));
        services.Configure<ReverseProxyConfig>(config.GetSection("ProxyConfig"));
        
        var connectionString = config.GetConnectionString("Default") ?? throw new NullReferenceException("Отсутствует строка подключения к БД");
        services.AddSingleton<ILiteDbContext>(_ => new DbContext(connectionString));
        
        services.AddHostedService<Starter>();

        services.AddScoped<IUserService, UserService>();
        services.AddTransient<IMoneyTransactionService, MoneyTransactionService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IBotManagementService, BotManagementService>();

        services.AddMediatR(typeof(HandleUpdateCommand).GetTypeInfo().Assembly);
        return services;
    }
    
    public static IServiceCollection AddBotServices(this IServiceCollection services)
    {
        services.AddHttpClient("tgwebhook")
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                var opt = sp.GetRequiredService<IOptions<BotConfiguration>>().Value;
                return new TelegramBotClient(opt.BotToken, httpClient);
            });
        services.AddScoped<IUpdateHandlerFactory, UpdateHandlerFactory>();
        return services;
    }

    public static WebApplication MapBotController(this WebApplication app)
    {
        var opt = app.Services.GetRequiredService<IOptions<BotConfiguration>>().Value;
        app.MapControllerRoute(
            name: "tgwebhook",
            pattern: $"bot/{opt.BotToken}/webhook", 
            new { controller = "WebHook", action = "Post" });
        return app;
    }
}