using Afonya.Domain.Exceptions;
using Afonya.Domain.Repositories;
using Afonya.Infrastructure.Contexts;
using Afonya.Infrastructure.Repositories;
using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Logic.Bot.CommandBuilders;
using Afonya.Bot.Logic.Bot.Commands.Start;
using Afonya.Bot.Logic.Bot.PipelineBehaviors.Auth;
using Afonya.Bot.Logic.Delegates;
using Afonya.Bot.Logic.Services;
using Afonya.Bot.Logic.Services.Pooling;
using Afonya.Web.BackgroundTasks;
using Common.Options;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Afonya.Api.Interfaces.Services;
using Afonya.Api.Logic.Services;
using Afonya.Api.Logic.Management.Commands.CreateUser;
using Microsoft.AspNetCore.Authentication.Cookies;
using Afonya.Shared.Interfaces;
using Afonya.Shared.Logic.Services;

namespace Afonya.Web.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddErrorHandling(this WebApplicationBuilder builder, IHostEnvironment env)
    {
        builder.Services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, ex) => !env.IsDevelopment();

                options.Map<AfonyaForbiddenException>(exception => new ProblemDetails
                {
                    Type = nameof(AfonyaForbiddenException),
                    Title = "Ошибка авторизации",
                    Detail = exception.Message,
                    Status = StatusCodes.Status403Forbidden,
                });

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

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder, IConfiguration config)
    {
        //config
        builder.Services.Configure<ReverseProxyConfig>(config.GetSection("ProxyConfig"));
        builder.Services.Configure<AppSettings>(config.GetSection("AppSettings"));

        //store
        var connectionString = config.GetConnectionString("Default")
            ?? throw new NullReferenceException("Отсутствует строка подключения к БД");
        builder.Services.AddSingleton(_ => new DbContext(connectionString));

        //shared logic
        builder.Services.AddTransient<IStatisticsService, StatisticsService>();

        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IMoneyTransactionRepository, MoneyTransactionRepository>();
        builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
        builder.Services.AddTransient<ICallbackRepository, CallbackRepository>();
        builder.Services.AddSingleton<IHashService, HashService>();

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
        {
            opt.LoginPath = "/Account/Login";
            opt.LogoutPath = "/Account/Logout";
        });

        //app
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(BotStartCommand).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateUserCommand).Assembly);
            cfg.AddOpenBehavior(typeof(BotAuthBehavior<,>));
        });
        builder.Services.AddHostedService<Starter>();
        builder.Services.AddRazorPages().AddRazorPagesOptions(opt =>
        {
            opt.Conventions.AddPageRoute("/Transactions/Index", "");
        }).AddNewtonsoftJson();
        return builder;
    }

    public static WebApplicationBuilder AddBotServices(this WebApplicationBuilder builder, IConfiguration config)
    {
        var botConfig = new BotConfiguration();
        var configSection = config.GetSection("BotConfiguration");
        configSection.Bind(botConfig);

        builder.Services.Configure<BotConfiguration>(configSection);

        builder.Services.AddTransient<CallbackQueryHandler>();
        builder.Services.AddTransient<EditedMessageHandler>();
        builder.Services.AddTransient<MessageHandler>();
        builder.Services.AddTransient<UnknownUpdateHandler>();
        builder.Services.AddTransient<IBotKeyboardService, BotKeyboardService>();

        builder.Services.AddTransient<CommandBuilderResolver>(sp => token =>
            token switch
            {
                UpdateType.CallbackQuery => sp.GetRequiredService<CallbackQueryHandler>(),
                UpdateType.EditedMessage => sp.GetRequiredService<EditedMessageHandler>(),
                UpdateType.Message => sp.GetRequiredService<MessageHandler>(),
                _ => sp.GetRequiredService<UnknownUpdateHandler>()
            });

        builder.Services.AddHttpClient("tgwebhook")
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                var opt = sp.GetRequiredService<IOptions<BotConfiguration>>().Value;
                return new TelegramBotClient(opt.BotToken, httpClient);
            });
        builder.Services.AddScoped<IUpdateHandler, UpdateHandler>();

        if (!botConfig.RunPooling) return builder;
        builder.Services.AddHostedService<PollingService>();

        return builder;
    }
}