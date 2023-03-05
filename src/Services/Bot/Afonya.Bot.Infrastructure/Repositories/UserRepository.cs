using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces;
using Afonya.Bot.Interfaces.Repositories;
using LiteDB;
using Microsoft.Extensions.Logging;

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

    public IEnumerable<TelegramUser> Get()
    {
        var users = _db.Database.GetCollection<TelegramUser>().FindAll();
        return users;
    }

    public TelegramUser? Get(string id)
    {
        var user = _db.Database.GetCollection<TelegramUser>().FindById(new ObjectId(id));
        if (user == null) return null;
        return user;
    }

    public TelegramUser? GetByName(string userName)
    {
        var user = _db.Database.GetCollection<TelegramUser>().FindOne(x=>x.Login == userName);
        if (user == null) return null;
        return user;
    }

    public TelegramUser? Create(TelegramUser user)
    {
        var id = _db.Database.GetCollection<TelegramUser>().Insert(user);
        var result = Get(id.AsObjectId.ToString());
        return result;
    }

    public bool Delete(string id)
    {
        var res = _db.Database.GetCollection<TelegramUser>().Delete(id);
        return res;
    }
}