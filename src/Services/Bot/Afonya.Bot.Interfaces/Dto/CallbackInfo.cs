using Newtonsoft.Json;

namespace Afonya.Bot.Interfaces.Dto;

public class CallbackInfo
{
    public string Id { get; set; }
    public string? MessageId { get; set; }
    public string Ctg { get; set; }

    public override string ToString()
    {
        var str = JsonConvert.SerializeObject(this);
        return str;
    }
}