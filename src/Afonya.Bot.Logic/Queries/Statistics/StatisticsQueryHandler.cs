using System.Text;
using Afonya.Domain.Entities;
using Afonya.Domain.Repositories;
using Afonya.Shared.Interfaces;
using MediatR;
using Shared.Contracts;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Afonya.Bot.Logic.Bot.Queries.Statistics;

public class StatisticsQueryHandler : IRequestHandler<StatisticQuery, bool>
{
    private readonly ITelegramBotClient _botClient;
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    private readonly IStatisticsService _statisticsService;

    public StatisticsQueryHandler(ITelegramBotClient botClient,
        IMoneyTransactionRepository moneyTransactionRepository,
        ICategoryRepository categoryRepository,
        IStatisticsService statisticsService)
    {
        _botClient = botClient;
        _moneyTransactionRepository = moneyTransactionRepository;
        _categoryRepository = categoryRepository;
        _statisticsService = statisticsService;
    }

    public async Task<bool> Handle(StatisticQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository.Get();
        var result = _moneyTransactionRepository.Get(request.Month, request.Year, null, null).ToArray();

        var month = $"{request.Month}".Length == 1 ? $"0{request.Month}" : $"{request.Month}";
        var message = _statisticsService.BuildStatisticSrt(result, categories, month, request.Year);
        await _botClient.DeleteMessage(request.ChatId, request.OriginalMessageId, cancellationToken: cancellationToken);
        await _botClient.SendMessage(request.ChatId, message, parseMode: ParseMode.Html, cancellationToken: cancellationToken);
        return true;
    }

}
