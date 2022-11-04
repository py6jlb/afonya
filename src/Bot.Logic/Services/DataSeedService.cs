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
        InitCategories(categoryService);
        var userService = _services.GetRequiredService<IUserService>();
        InitUsers(userService);
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
            .Get<CategoryDto[]>().Select(x => { x.IsActive = true; return x; });
        foreach (var categoryDto in categories)
        {
            srv.AddCategory(categoryDto);
        }
    }

    private void InitUsers(IUserService srv)
    {
        var notNull = srv.UsersExists();
        var users = _configuration.GetSection("Users").Get<UserDto[]>();
        foreach (var usersDto in users)
        {
            srv.AddUser(usersDto);
        }
    }
}