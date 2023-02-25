namespace Shared.Contracts;

public class CategoryDto
{
    public string? Id { get; set; }
    public string Icon { get; set; }
    public string Name { get; set; }
    public string HumanName { get; set; }
    public bool IsActive { get; set; }
}