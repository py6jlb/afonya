using Afonya.MoneyBot.Infrastructure.Contexts;
using Afonya.MoneyBot.Interfaces;
using Afonya.MoneyBot.Interfaces.Dto;
using Afonya.MoneyBot.Interfaces.Services;
using Afonya.MoneyBot.Logic.Services;
using Common.Exceptions;
using Common.Options;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Afonya.MoneyBot.WebWorker.Extensions;

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
        services.AddHostedService<Starter>();

        services.AddSingleton<ILiteDbContext>(p => new DbContext(connectionString));
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<IMoneyTransactionService, MoneyTransactionService>();
        services.AddTransient<ICategoryService, CategoryService>();
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
        services.AddScoped<IHandleUpdateService, HandleUpdateService>();
        return services;
    }

    public static WebApplication MapBotController(this WebApplication app)
    {
        var opt = app.Services.GetRequiredService<IOptions<BotConfiguration>>().Value;
        app.MapControllerRoute(
            name: "tgwebhook",
            pattern: $"bot/{opt.BotToken}", 
            new { controller = "WebHook", action = "Post" });
        return app;
    }
}