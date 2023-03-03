using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces;
using Afonya.Bot.Interfaces.Repositories;
using Common.Exceptions;
using LiteDB;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace Afonya.Bot.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ILogger<CategoryRepository> _logger;
    private readonly LiteDatabase _db;

    public CategoryRepository(ILogger<CategoryRepository> logger, ILiteDbContext context)
    {
        _logger = logger;
        _db = context.Database;
    }
    
    public IReadOnlyCollection<CategoryDto> Get(bool onlyActive = true)
    {
        var t =  onlyActive ? _db.GetCollection<Category>().Find(x => x.IsActive == true) 
            : _db.GetCollection<Category>().FindAll();

        return t.Select(x => new CategoryDto
        {
            Id = x.Id.ToString(),
            Name = x.Name,
            HumanName = x.HumanName,
            Icon = x.Icon,
            IsActive = x.IsActive
        }).ToArray();
    }

    public CategoryDto? Get(string id)
    {
        var cat = _db.GetCollection<Category>().FindById(new ObjectId(id));
        if (cat == null) return null;

        return new CategoryDto
        {
            Id = cat.Id.ToString(),
            Name = cat.Name,
            HumanName = cat.HumanName,
            Icon = cat.Icon,
            IsActive = cat.IsActive
        };
    }

    public int Count()
    {
        var res = _db.GetCollection<Category>().Count();
        return res;
    }

    public CategoryDto Create(CategoryDto category)
    {
        var entity = new Category
        {
            Name = category.Name,
            HumanName = category.HumanName,
            Icon = category.Icon,
            IsActive = category.IsActive
        };
        var res = _db.GetCollection<Category>().Insert(entity);
        var newCategory = Get(res.AsObjectId.ToString());
        return newCategory;
    }

    public CategoryDto Update(CategoryDto category)
    {
        if (string.IsNullOrWhiteSpace(category.Icon))
            throw new AfonyaErrorException("Отсутсвует id категории для обновления.");
        
        var cat = _db.GetCollection<Category>().FindById(new ObjectId(category.Id));
        cat.Icon = category.Icon;
        cat.Name = category.Name;
        cat.HumanName = category.HumanName;
        cat.IsActive = category.IsActive;

        var result = _db.GetCollection<Category>().Update(cat);
        if (result) return category;
        throw new AfonyaErrorException("Ошибка обновления категории.");
    }

    public bool Delete(string id)
    {
        var cat = _db.GetCollection<Category>().FindById(new ObjectId(id));
        cat.IsActive = false;
        var result = _db.GetCollection<Category>().Update(cat);
        return result;
    }
}