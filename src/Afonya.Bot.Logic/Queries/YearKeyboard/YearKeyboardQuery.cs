namespace Afonya.Bot.Logic.Bot.Queries.YearKeyboard;

public sealed record YearKeyboardQuery : BaseBotCommand<bool>
{
    public int OriginalMessageId { get; set; }
    public int? Year { get; set; }
};