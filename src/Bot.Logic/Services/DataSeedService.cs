using Bot.Interfaces.Dto;
using Bot.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bot.Logic.Services;

public class DataSeedService : IHostedService
{
    private readonly ILogger<DataSeedService> _logger;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    
    public DataSeedService(
        ILogger<DataSeedService> logger, 
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
        var notNull = categoryService.CategoriesExists();
        if (notNull) return Task.CompletedTask;
        
        var categories = _configuration.GetSection("Categories")
            .Get<CategoryDto[]>().Select(x => { x.IsActive = true; return x; });
        foreach (var categoryDto in categories)
        {
            categoryService.AddCategory(categoryDto);
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}