using Afonya.MoneyBot.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace Afonya.MoneyBot.Logic.Services;

public class Starter : IHostedService
{
    private readonly ILogger<Starter> _logger;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    
    public Starter(
        ILogger<Starter> logger, 
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = serviceProvider;
        _configuration = configuration;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var categoryService = _services.GetRequiredService<ICategoryService>();
        InitCategories(categoryService);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void InitCategories(ICategoryService srv)
    {
        var notNull = srv.CategoriesExists();
        if (notNull) return;
        
        var categories = _configuration.GetSection("Categories")
            .Get<CategoryDto[]>()?.Select(x => { x.IsActive = true; return x; }) ?? Array.Empty<CategoryDto>();

        foreach (var categoryDto in categories)
        {
            srv.Create(categoryDto);
        }
    }
}