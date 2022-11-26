using Common.Exceptions;
using LiteDB;
using Microsoft.Extensions.Logging;
using MoneyBot.Domain.Entities;
using MoneyBot.Interfaces;
using MoneyBot.Interfaces.Dto;
using MoneyBot.Interfaces.Services;

namespace MoneyBot.Logic.Services;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly LiteDatabase _db;
    private ICategoryService _categoryServiceImplementation;

    public CategoryService(ILogger<CategoryService> logger, ILiteDbContext context)
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
    public CategoryDto Get(string id)
    {
        var cat = _db.GetCollection<Category>().FindById(id);

        return  new CategoryDto
        {
            Id = cat.Id.ToString(),
            Name = cat.Name,
            HumanName = cat.HumanName,
            Icon = cat.Icon,
            IsActive = cat.IsActive
        };
    }

    public bool CategoriesExists()
    {
        var res = _db.GetCollection<Category>().Count() > 0;
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
        var newCat = Get(res.ToString());
        return newCat;
    }

    public CategoryDto Update(CategoryDto category)
    {
        if (string.IsNullOrWhiteSpace(category.Icon))
            throw new AfonyaErrorException("Отсутсвует id категории для обновления.");
        
        var cat = _db.GetCollection<Category>().FindById(category.Id);
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
        var cat = _db.GetCollection<Category>().FindById(id);
        cat.IsActive = false;
        var result = _db.GetCollection<Category>().Update(cat);
        return result;
    }
}