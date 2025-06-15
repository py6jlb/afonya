using Afonya.Domain.Entities;

namespace Afonya.Bot.Interfaces.Dto.CallbackData;

public record SetCategoryCallbackData
{
    public string DataId { get; set; }
    public string CategoryId { get; set; }
    public string Icon { get; set; }
    public string Name { get; set; }
    public string HumanName { get; set; }
}