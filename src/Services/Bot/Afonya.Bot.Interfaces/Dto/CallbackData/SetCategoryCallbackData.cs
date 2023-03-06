using Afonya.Bot.Domain.Entities;

namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public record SetCategoryCallbackData
{
    public string DataId { get; set; }
    public Category Category { get; set; }
}