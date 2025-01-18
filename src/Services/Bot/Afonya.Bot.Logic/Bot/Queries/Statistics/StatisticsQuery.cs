namespace Afonya.Bot.Logic.Bot.Queries.Statistics;

public sealed record StatisticQuery : BaseBotCommand<bool>
{
    public int? Month { get; set; }
    public int? Year { get; set; }
    public int OriginalMessageId { get; set; }
};