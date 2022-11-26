using LiteDB;

namespace Afonya.MoneyBot.Domain.Entities;

public class Category
{
    public ObjectId Id { get; set; }
    public string Icon { get; set; }
    public string Name { get; set; }
    public string HumanName { get; set; }
    public bool IsActive { get; set; }
}