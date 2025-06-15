using Afonya.Domain.Entities;
using Afonya.Shared.Interfaces.Dto;

namespace Afonya.Shared.Interfaces;

public interface IStatisticsService
{
    string BuildStatisticSrt(IEnumerable<MoneyTransaction> transactions, IEnumerable<Category> categories, string? month, int? year);
    StatisticsDto BuildStatistic(IEnumerable<MoneyTransaction> transactions, IEnumerable<Category> categories);
}
