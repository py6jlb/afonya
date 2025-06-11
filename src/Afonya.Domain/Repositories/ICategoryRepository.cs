using Afonya.Domain.Entities;

namespace Afonya.Domain.Repositories;

public interface ICategoryRepository
{
    IEnumerable<Category> Get(bool onlyActive = true);
    public Category? Get(string id);
    int Count();
    Category Create(Category category);
    Category Update(Category category);
    bool Delete(string id);
}