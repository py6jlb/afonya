using System.ComponentModel.DataAnnotations;
using Common.Exceptions;
using Common.Extensions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using MoneyBot.Infrastructure.Contexts;
using MoneyBot.Interfaces;
using MoneyBot.Interfaces.Dto;
using MoneyBot.Interfaces.Services;
using MoneyBot.Logic.Services;
using Telegram.Bot;

namespace MoneyBot.WebWorker.Extensions;

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
        services.AddHostedService<DataSeedService>();
        services.AddJwtAuthentication(config);
        services.AddSingleton<ILiteDbContext>(p => new DbContext(config.GetConnectionString("Default")));
        services.AddScoped<IUserService, UserService>();
        services.AddTransient<IMoneyTransactionService, MoneyTransactionService>();
        services.AddTransient<ICategoryService, CategoryService>();
        return services;
    }
    
    public static IServiceCollection AddBotServices(this IServiceCollection services, BotConfiguration config)
    {
        services.AddHttpClient("tgwebhook")
            .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(config.BotToken, httpClient));
        services.AddScoped<IHandleUpdateService, HandleUpdateService>();
        return services;
    }

    public static ControllerActionEndpointConventionBuilder MapBotController(this IEndpointRouteBuilder endpoints,
        BotConfiguration config)
    {
        var result = endpoints.MapControllerRoute(
            name: "tgwebhook",
            pattern: $"bot/{config.BotToken}", 
            new { controller = "WebHook", action = "Post" });
        return result;
    }
}