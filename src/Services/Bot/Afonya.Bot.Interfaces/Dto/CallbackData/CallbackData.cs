using Afonya.Bot.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public record CallbackData
{
    public CallbackCommand Command { get; set; }
    public string Serialize()
    {
        var converter = new StringEnumConverter();
        var str = JsonConvert.SerializeObject(this, converter);
        return str; 
    }
}