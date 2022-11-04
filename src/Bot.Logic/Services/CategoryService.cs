using Bot.Domain.Entities;
using Bot.Interfaces;
using Bot.Interfaces.Dto;
using Bot.Interfaces.Services;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Bot.Logic.Services;

public class CategoryService : ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly LiteDatabase _db;
    public CategoryService(ILogger<CategoryService> logger, ILiteDbContext context)
    {
        _logger = logger;
        _db = context.Database;
    }
    
    public IEnumerable<CategoryDto> Get(bool onlyActive = true)
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
        });
    }
}