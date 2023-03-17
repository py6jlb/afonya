using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Domain.Exceptions;
using Afonya.Bot.Infrastructure.Contexts;
using Afonya.Bot.Interfaces.Repositories;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Afonya.Bot.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ILogger<CategoryRepository> _logger;
    private readonly LiteDatabase _db;

    public CategoryRepository(ILogger<CategoryRepository> logger, DbContext context)
    {
        _logger = logger;
        _db = context.Database;
    }
    
    public IEnumerable<Category> Get(bool onlyActive = true)
    {
        var result =  onlyActive ? _db.GetCollection<Category>().Find(x => x.IsActive == true) 
            : _db.GetCollection<Category>().FindAll();

        return result;
    }

    public Category? Get(string id)
    {
        var category = _db.GetCollection<Category>().FindById(new ObjectId(id));
        return category;
    }

    public int Count()
    {
        var res = _db.GetCollection<Category>().Count();
        return res;
    }

    public Category Create(Category category)
    {
        var id = _db.GetCollection<Category>().Insert(category);
        var result = Get(id.AsObjectId.ToString());
        return result;
    }

    public Category Update(Category update)
    {
        if (string.IsNullOrWhiteSpace(update.Icon))
            throw new AfonyaErrorException("Отсутсвует id категории для обновления.");
        
        var result = _db.GetCollection<Category>().Update(update);
        if (result) return update;
        throw new AfonyaErrorException("Ошибка обновления категории.");
    }

    public bool Delete(string id)
    {
        var category = _db.GetCollection<Category>().FindById(new ObjectId(id));
        category.SetIsActive(false);
        var result = _db.GetCollection<Category>().Update(category);
        return result;
    }
}