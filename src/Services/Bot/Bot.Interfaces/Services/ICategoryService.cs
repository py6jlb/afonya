using Bot.Interfaces.Dto;

namespace Bot.Interfaces.Services;

public interface ICategoryService
{
    IEnumerable<CategoryDto> Get(bool onlyActive = true);
    bool CategoriesExists();
    string AddCategory(CategoryDto category);
}