using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces;
using Afonya.Bot.Interfaces.Repositories;
using LiteDB;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace Afonya.Bot.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly ILiteDbContext _db;
    
    public UserRepository(ILogger<UserRepository> logger, ILiteDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public int Count()
    {
        var res = _db.Database.GetCollection<TelegramUser>().Count();
        return res;
    }

    public IReadOnlyCollection<UserDto> Get()
    {
        var users = _db.Database.GetCollection<TelegramUser>().FindAll()
            .Select(x => new UserDto(x.Id.ToString(), x.Login)).ToArray();
        return users;
    }

    public UserDto? Get(string id)
    {
        var user = _db.Database.GetCollection<TelegramUser>().FindById(new ObjectId(id));
        if (user == null) return null;
        var res = new UserDto(user.Id.ToString(), user.Login);
        return res;
    }

    public UserDto? GetByName(string userName)
    {
        var user = _db.Database.GetCollection<TelegramUser>().FindOne(x=>x.Login == userName);
        if (user == null) return null;
        var res = new UserDto(user.Id.ToString(), user.Login);
        return res;
    }

    public UserDto? Create(UserDto user)
    {
        var entity = new TelegramUser
        {
            Login = user.Login
        };

        var id = _db.Database.GetCollection<TelegramUser>().Insert(entity);
        var result = Get(id.AsObjectId.ToString());
        return result;
    }

    public bool Delete(string id)
    {
        var res = _db.Database.GetCollection<TelegramUser>().Delete(id);
        return res;
    }
}