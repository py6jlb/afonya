namespace Afonya.Bot.Domain.Entities;

public class Category : BaseEntity
{
    protected Category() { }

    public Category(string icon, string name, string humanName, bool isActive)
    {
        Icon = icon;
        Name = name;
        HumanName = humanName;
        IsActive = isActive;
    }

    public string Icon { get; private set; }
    public string Name { get; private set; }
    public string HumanName { get; private set; }
    public bool IsActive { get; private set; }

    public void SetIcon(string icon)
    {
        Icon = icon;
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetHumanName(string humanName)
    {
        HumanName = humanName;
    }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
}