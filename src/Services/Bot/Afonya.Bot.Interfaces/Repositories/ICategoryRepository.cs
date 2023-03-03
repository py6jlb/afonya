using Shared.Contracts;

namespace Afonya.Bot.Interfaces.Repositories;

public interface ICategoryRepository
{
    IReadOnlyCollection<CategoryDto> Get(bool onlyActive = true);
    public CategoryDto? Get(string id);
    int Count();
    CategoryDto Create(CategoryDto category);
    CategoryDto Update(CategoryDto category);
    bool Delete(string id);
}