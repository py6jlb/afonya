using Afonya.Bot.Domain.Enums;
using Newtonsoft.Json;

namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public record CallbackData
{
    public CallbackCommand Command { get; set; }
    public override string ToString()
    {
        var str = JsonConvert.SerializeObject(this, new JsonSerializerSettings());
        return str;
    }
}