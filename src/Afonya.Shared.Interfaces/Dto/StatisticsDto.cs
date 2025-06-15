using System;

namespace Afonya.Shared.Interfaces.Dto;

public class StatisticsDto
{
    public double TotalMinus { get; set; } = 0;
    public double TotalPlus { get; set; } = 0;
    public IEnumerable<StatisticItem> ByCategories { get; set; }
}


public class StatisticItem
{
    public string CategoryId { get; set; }
    public string CategoryIcon { get; set; }
    public string CategoryName { get; set; }

    public double Minus { get; set; } = 0;
    public double MinusPercent { get; set; } = 0;

    public double Plus { get; set; } = 0;
    public double PlusPercent { get; set; } = 0;
}