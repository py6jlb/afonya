using Afonya.Api.Logic.Categories.Commands.AddCategory;
using Afonya.Api.Logic.Categories.Queries.GetCategoryCount;
using Afonya.Api.Logic.Management.Commands.CreateUser;
using Afonya.Bot.Interfaces.Dto;
using MediatR;
using Shared.Contracts;

namespace Afonya.Web.BackgroundTasks;

public class Starter : IHostedService
{
    private readonly ILogger<Starter> _logger;
    private readonly IConfiguration _configuration;

    public Starter(
        ILogger<Starter> logger,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        Scope = scopeFactory.CreateScope();
    }

    private IServiceScope Scope { get; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await InitCategories(cancellationToken);
        await InitUsers(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Завершение работы приложение");
        Scope.Dispose();
        return Task.CompletedTask;
    }

    private async Task InitCategories(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Инициализация категорий");
        var mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();
        var count = await mediator.Send(new GetCategoriesCountQuery(), cancellationToken);
        if (count > 0)
        {
            _logger.LogDebug("В БД уже есть категории, инициализация пропущена.");
            return;
        }
        var categories = _configuration.GetSection("Categories").GetChildren().ToArray();
        _logger.LogDebug("Получено {Count} категорий, из файла конфигурации.", categories.Count());

        foreach (var category in categories)
        {
            var categoryDto = category.Get<CategoryDto>();
            if (categoryDto == null) continue;
            categoryDto.IsActive = true;
            await mediator.Send(new AddCategoryCommand
            {
                NewCategory = categoryDto
            }, cancellationToken);
        }
    }

    public async Task InitUsers(CancellationToken cancellationToken)
    {
        var mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();
        var configuration = Scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var users = configuration.GetSection("Users").Get<UserConfig[]>() ?? [];
        foreach (var userConfig in users)
        {
            try
            {
                await mediator.Send(new CreateUserCommand
                {
                    Login = userConfig.Username,
                    Password = userConfig.Password,
                    IsAdmin = userConfig.IsAdmin
                }, cancellationToken);
                _logger.LogInformation("Пользователь создан: {username}", userConfig.Username);
            }
            catch (Exception error)
            {
                _logger.LogInformation("Пользователь не создан: {message}", error.Message);
            }
        }
    }
}