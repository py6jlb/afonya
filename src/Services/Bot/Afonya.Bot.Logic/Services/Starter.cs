﻿using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Contracts;
using Telegram.Bot;

namespace Afonya.Bot.Logic.Services;

public class Starter : IHostedService
{
    private readonly ILogger<Starter> _logger;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    private readonly ITelegramBotClient _botClient;
    private readonly bool _usePooling;
    
    public Starter(
        ILogger<Starter> logger, 
        IServiceProvider serviceProvider,
        IConfiguration configuration, ITelegramBotClient botClient)
    {
        _logger = logger;
        _services = serviceProvider;
        _configuration = configuration;
        _botClient = botClient;
        _usePooling = configuration.GetSection("BotConfiguration:UsePooling").Get<bool>();
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var categories = _services.GetRequiredService<ICategoryRepository>();
        InitCategories(categories);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if(!_usePooling) await _botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }

    private void InitCategories(ICategoryRepository repo)
    {
        var count = repo.Count();
        if (count > 0) return;

        var categories = _configuration.GetSection("Categories").GetChildren();

        foreach (var category in categories)
        {
            var categoryDto = category.Get<CategoryDto>();
            if (categoryDto == null) continue;
            var newCategory = new Category(categoryDto.Icon, categoryDto.Name, categoryDto.HumanName, true);
            repo.Create(newCategory);
        }
    }
}