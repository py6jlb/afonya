using Shared.Contracts;

namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public record SetCategoryCallbackData
{
    public string DataId { get; set; }
    public CategoryDto Category { get; set; }
}