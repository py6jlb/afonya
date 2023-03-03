namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public record SetCategory : CallbackData
{
    public string DataId { get; set; }
    public string Ctg { get; set; }
}