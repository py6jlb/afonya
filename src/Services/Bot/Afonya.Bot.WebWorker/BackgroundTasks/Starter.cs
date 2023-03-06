using Afonya.Bot.Logic.Commands.Bot.BotStop;
using Afonya.Bot.Logic.Commands.Categories.AddCategory;
using Afonya.Bot.Logic.Queries.GetCategoryCount;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.WebWorker.BackgroundTasks;

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
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Завершение работы приложение, попытка отключить webhook.");
        var mediator = Scope.ServiceProvider.GetRequiredService<IMediator>();
        await mediator.Send(new BotStopCommand(), cancellationToken);
        Scope.Dispose();

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
}