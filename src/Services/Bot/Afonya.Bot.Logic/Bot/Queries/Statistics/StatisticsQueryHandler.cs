using Afonya.Bot.Domain.Repositories;
using Afonya.Bot.Interfaces.Services;
using MediatR;
using Shared.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Afonya.Bot.Logic.Bot.Queries.Statistics;

public class StatisticsQueryHandler : IRequestHandler<StatisticQuery, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IChartService _chartService;

    public StatisticsQueryHandler(ITelegramBotClient botClient,
        IMoneyTransactionRepository moneyTransactionRepository,
        ICategoryRepository categoryRepository,
        IChartService chartService)
    {
        _botClient = botClient;
        _moneyTransactionRepository = moneyTransactionRepository;
        _categoryRepository = categoryRepository;
        _chartService = chartService;
    }

    public async Task<bool> Handle(StatisticQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository.Get();
        var result = _moneyTransactionRepository.Get(request.Month, request.Year, null, null).Select(x => new MoneyTransactionDto
        {
            Id = x.Id?.ToString(),
            CategoryName = x.CategoryName,
            CategoryHumanName = x.CategoryHumanName,
            CategoryIcon = x.CategoryIcon,
            Value = x.Value,
            Sign = x.Sign,
            RegisterDate = x.RegisterDate,
            TransactionDate = x.TransactionDate,
            FromUserName = x.FromUserName
        }).ToArray();

        var month = $"{request.Month}".Length == 1 ? $"0{request.Month}" : $"{request.Month}";
        var filePath = await _chartService.GetStatisticPng($"Статистика за {month}.{request.Year}", result, categories);
        await _botClient.DeleteMessage(request.ChatId, request.OriginalMessageId, cancellationToken: cancellationToken);
        await using Stream stream = System.IO.File.OpenRead(filePath);
        await _botClient.SendPhoto(request.ChatId, stream, cancellationToken: cancellationToken);
        System.IO.File.Delete(filePath);
        return true;
    }
}
