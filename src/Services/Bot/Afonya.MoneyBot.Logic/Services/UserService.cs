using Afonya.MoneyBot.Domain.Entities;
using Afonya.MoneyBot.Interfaces;
using Afonya.MoneyBot.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace Afonya.MoneyBot.Logic.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly ILiteDbContext _db;
    
    public UserService(ILogger<UserService> logger, ILiteDbContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    public bool UsersExists()
    {
        var res = _db.Database.GetCollection<User>().Count() > 0;
        return res;
    }

    public IReadOnlyCollection<UserDto> Get()
    {
        var users = _db.Database.GetCollection<User>().FindAll().Select(x => new UserDto
        {
            Id = x.Id.ToString(),
            Login = x.Login
        }).ToArray();
        return users;
    }

    public UserDto? Get(string id)
    {
        var user = _db.Database.GetCollection<User>().FindById(id);
        if (user == null) return null;
        var res = new UserDto
        {
            Id = user.Id.ToString(),
            Login = user.Login
        };
        return res;
    }

    public UserDto? Create(UserDto user)
    {
        var entity = new User
        {
            Login = user.Login
        };

        var id = _db.Database.GetCollection<User>().Insert(entity);
        var result = Get(id);
        return result;
    }

    public bool Delete(string id)
    {
        var res = _db.Database.GetCollection<User>().Delete(id);
        return res;
    }
}