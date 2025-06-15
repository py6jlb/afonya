using System;
using System.Text;
using Afonya.Domain.Entities;
using Afonya.Shared.Interfaces;
using Afonya.Shared.Interfaces.Dto;

namespace Afonya.Shared.Logic.Services;

public class StatisticsService : IStatisticsService
{
    public string BuildStatisticSrt(IEnumerable<MoneyTransaction> transactions, IEnumerable<Category> categories, string? month, int? year)
    {
        var sb = new StringBuilder();
        sb.Append("<b>Статистика за ");
        if (!string.IsNullOrWhiteSpace(month))
        {
            sb.Append($"{month}.");
        }
        if (!string.IsNullOrWhiteSpace(month))
        {
            sb.Append($"{year}:");
        }
        sb.Append("</b>");
        sb.AppendLine();
        var totalMinusRaw = transactions.Where(x => x.Sign == "-").Select(x => 0 - x.Value).Sum();
        var totalMinusValue = Math.Round(totalMinusRaw, 2);

        sb.AppendLine($"");
        sb.AppendLine($"<b>Всего потрачено: {totalMinusValue} руб</b>");
        foreach (var c in categories)
        {
            var categoryRaw = transactions.Where(x => x.CategoryName == c.Name && x.Sign == "-").Select(x => 0 - x.Value).Sum();
            var categoryValue = Math.Round(categoryRaw, 2);
            var percents = categoryValue / totalMinusValue * 100;
            if (categoryValue == 0)
            {
                sb.AppendLine($"{c.Icon} {c.HumanName}: <b>-</b>");
            }
            else
            {
                sb.AppendLine($"{c.Icon} {c.HumanName}: <b>{Math.Round(percents, 1)}%</b> (<i>{Math.Round(categoryValue, 2)} руб</i>)");
            }
        }

        var totalPlusRaw = transactions.Where(x => x.Sign == "+").Select(x => x.Value).Sum();
        var totalPlusValue = Math.Round(totalPlusRaw, 2);
        if (totalPlusValue != 0)
        {
            sb.AppendLine($"");
            sb.AppendLine($"<b>Всего получено: {totalPlusValue} руб</b>");
            foreach (var c in categories)
            {
                var categoryRaw = transactions.Where(x => x.CategoryName == c.Name && x.Sign == "+").Select(x => x.Value).Sum();
                var categoryValue = Math.Round(categoryRaw, 2);
                var percents = categoryValue / totalPlusValue * 100;
                if (categoryValue == 0)
                {
                    sb.AppendLine($"{c.Icon} {c.HumanName}: <b>-</b>");
                }
                else
                {
                    sb.AppendLine($"{c.Icon} {c.HumanName}: <b>{Math.Round(percents, 1)}%</b> (<i>{Math.Round(categoryValue, 2)} руб</i>)");
                }
            }
        }

        return sb.ToString();
    }

    public StatisticsDto BuildStatistic(IEnumerable<MoneyTransaction> transactions, IEnumerable<Category> categories)
    {
        var result = new StatisticsDto();

        var totalMinusRaw = transactions.Where(x => x.Sign == "-").Select(x => 0 - x.Value).Sum();
        result.TotalMinus = Math.Round(totalMinusRaw, 2);

        var totalPlusRaw = transactions.Where(x => x.Sign == "+").Select(x => x.Value).Sum();
        result.TotalPlus = Math.Round(totalPlusRaw, 2);

        var items = new List<StatisticItem>();

        foreach (var c in categories)
        {
            var categoryMinusRaw = transactions.Where(x => x.CategoryName == c.Name && x.Sign == "-").Select(x => 0 - x.Value).Sum();
            var categoryMinusValue = Math.Round(categoryMinusRaw, 2);
            var minusPercents = result.TotalMinus != 0 ? categoryMinusValue / result.TotalMinus * 100 : 0;

            var categoryPlusRaw = transactions.Where(x => x.CategoryName == c.Name && x.Sign == "+").Select(x => x.Value).Sum();
            var categoryPlusValue = Math.Round(categoryPlusRaw, 2);
            var plusPercents = result.TotalPlus != 0 ? categoryPlusValue / result.TotalPlus * 100 : 0;

            var item = new StatisticItem
            {
                CategoryIcon = c.Icon,
                CategoryId = c.Id.ToString(),
                CategoryName = c.HumanName,
                Minus = categoryMinusValue,
                MinusPercent = Math.Round(minusPercents, 2),
                Plus = categoryPlusValue,
                PlusPercent = Math.Round(plusPercents, 2),
            };
            items.Add(item);
        }
        result.ByCategories = items;

        return result;
    }
}
