namespace Afonya.Bot.Logic.Bot.Commands.NewMessage;

public sealed record NewMessageCommand :BaseBotCommand<bool>
{
    public int MessageId { get; set; }
    public string MessageText { get; set; }
    public DateTime MessageDate { get; set; }
}