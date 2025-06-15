using System;
using Afonya.Shared.Interfaces.Dto;
using MediatR;

namespace Afonya.ApiLogic.Statistics.Queries.GetStatistics;

public class GetStatisticsQuery: IRequest<StatisticsDto>
{
    public int? Month { get; set; }
    public int? Year { get; set; }
}
