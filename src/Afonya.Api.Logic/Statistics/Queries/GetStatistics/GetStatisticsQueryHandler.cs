using System;
using Afonya.ApiLogic.Statistics.Queries.GetStatistics;
using Afonya.Domain.Repositories;
using Afonya.Shared.Interfaces;
using Afonya.Shared.Interfaces.Dto;
using MediatR;

namespace Afonya.Api.Logic.Statistics.Queries.GetStatistics;

public class GetStatisticsQueryHandler : IRequestHandler<GetStatisticsQuery, StatisticsDto>
{
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    private readonly IStatisticsService _statisticsService;


    public GetStatisticsQueryHandler(IMoneyTransactionRepository moneyTransactionRepository, ICategoryRepository categoryRepository, IStatisticsService statisticsService)
    {
        _categoryRepository = categoryRepository;
        _moneyTransactionRepository = moneyTransactionRepository;
        _statisticsService = statisticsService;
    }
    public Task<StatisticsDto> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository.Get();
        var result = _moneyTransactionRepository.Get(request.Month, request.Year, null, null).ToArray();
        var res = _statisticsService.BuildStatistic(result, categories);

        return Task.FromResult(res);
    }
}
