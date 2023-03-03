﻿using System.Reflection;
using Afonya.Bot.Infrastructure.Contexts;
using Afonya.Bot.Infrastructure.Repositories;
using Afonya.Bot.Interfaces;
using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Repositories;
using Afonya.Bot.Interfaces.Services;
using Afonya.Bot.Interfaces.Services.UpdateHandler;
using Afonya.Bot.Logic.Commands.Bot.NewTelegramEvent;
using Afonya.Bot.Logic.Services;
using Afonya.Bot.Logic.Services.Pooling;
using Common.Exceptions;
using Common.Options;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using IUpdateHandler = Telegram.Bot.Polling.IUpdateHandler;

namespace Afonya.Bot.WebWorker.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddErrorHandling(this WebApplicationBuilder builder, IHostEnvironment env)
    {
        builder.Services.AddProblemDetails(options =>
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

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder, IConfiguration config)
    {
        //config
        builder.Services.Configure<AdminUser>(config.GetSection("AdminUser"));
        builder.Services.Configure<ReverseProxyConfig>(config.GetSection("ProxyConfig"));
        
        //store
        var connectionString = config.GetConnectionString("Default") ?? throw new NullReferenceException("Отсутствует строка подключения к БД");
        builder.Services.AddSingleton<ILiteDbContext>(_ => new DbContext(connectionString));
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IMoneyTransactionRepository, MoneyTransactionRepository>(); 
        builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

        //app
        builder.Services.AddTransient<IBotKeyboardService, BotKeyboardService>();
        builder.Services.AddTransient<IBotManagementService, BotManagementService>();
        builder.Services.AddHostedService<Starter>();
        builder.Services.AddMediatR(typeof(NewTelegramEventCommand).GetTypeInfo().Assembly);
        builder.Services.AddControllers().AddNewtonsoftJson();
        return builder;
    }
    
    public static WebApplicationBuilder AddBotServices(this WebApplicationBuilder builder, IConfiguration config)
    {
        var botConfig = new BotConfiguration();
        var configSection = config.GetSection("BotConfiguration");
        configSection.Bind(botConfig);

        builder.Services.Configure<BotConfiguration>(configSection);
        builder.Services.AddHttpClient("tgwebhook")
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                var opt = sp.GetRequiredService<IOptions<BotConfiguration>>().Value;
                return new TelegramBotClient(opt.BotToken, httpClient);
            });

        builder.Services.AddScoped<IUpdateHandlerFactory, UpdateHandlerFactory>();
        if (!botConfig.UsePooling) return builder;

        builder.Services.AddScoped<IUpdateHandler, UpdateHandler>();
        builder.Services.AddHostedService<PollingService>();

        return builder;
    }
}