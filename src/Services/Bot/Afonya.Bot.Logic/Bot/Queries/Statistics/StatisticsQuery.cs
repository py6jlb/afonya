namespace Afonya.Bot.Logic.Bot.Queries.Statistics;

public sealed record StatisticQuery : BaseBotCommand<bool>
{
    public int? Month { get; set; }
    public int? Year { get; set; }
    public string? User { get; set; }
    public string? Category { get; set; }
};