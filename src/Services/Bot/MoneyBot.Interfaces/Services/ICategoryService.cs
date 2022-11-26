using MoneyBot.Interfaces.Dto;

namespace MoneyBot.Interfaces.Services;

public interface ICategoryService
{
    IReadOnlyCollection<CategoryDto> Get(bool onlyActive = true);
    public CategoryDto Get(string id);
    bool CategoriesExists();
    CategoryDto Create(CategoryDto category);
    CategoryDto Update(CategoryDto category);
    bool Delete(string id);
}