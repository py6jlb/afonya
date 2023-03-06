namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public record DeleteRequestCallbackData
{
    public string DataId { get; set; }
    public string OriginalMessageText { get; set; }
    public bool Confirm { get; init; }
}