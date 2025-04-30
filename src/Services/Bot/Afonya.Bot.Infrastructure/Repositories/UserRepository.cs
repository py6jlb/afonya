using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Domain.Repositories;
using Afonya.Bot.Infrastructure.Contexts;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Afonya.Bot.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly ILiteDatabase _db;

    public UserRepository(ILogger<UserRepository> logger, DbContext context)
    {
        _logger = logger;
        _db = context.Database;
    }

    public int Count()
    {
        var res = _db.GetCollection<TelegramUser>().Count();
        return res;
    }

    public IEnumerable<TelegramUser> Get()
    {
        var users = _db.GetCollection<TelegramUser>().FindAll();
        return users;
    }

    public TelegramUser? Get(string id)
    {
        var user = _db.GetCollection<TelegramUser>().FindById(new ObjectId(id));
        return user ?? null;
    }

    public TelegramUser? GetByName(string userName)
    {
        var user = _db.GetCollection<TelegramUser>().FindOne(x => x.Login == userName);
        return user ?? null;
    }

    public TelegramUser? Create(TelegramUser user)
    {
        var id = _db.GetCollection<TelegramUser>().Insert(user);
        var result = Get(id.AsObjectId.ToString());
        return result;
    }

    public bool Delete(string id)
    {
        var res = _db.GetCollection<TelegramUser>().Delete(new ObjectId(id));
        return res;
    }

    public TelegramUser? Authenticate(string username, string password)
    {
        var user = _db.GetCollection<TelegramUser>().FindOne(x => x.Login == username && x.Password == password);
        return user ?? null;
    }

    public TelegramUser? ChangePassword(string id, string password)
    {
        var user = _db.GetCollection<TelegramUser>().FindById(new ObjectId(id));
        user.SetPassword(password);
        _db.GetCollection<TelegramUser>().Update(user);
        return user;
    }
}