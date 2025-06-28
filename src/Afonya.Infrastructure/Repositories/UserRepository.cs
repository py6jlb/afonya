using Afonya.Domain.Entities;
using Afonya.Domain.Exceptions;
using Afonya.Domain.Repositories;
using Afonya.Infrastructure.Contexts;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Afonya.Infrastructure.Repositories;

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
        var res = _db.GetCollection<User>().Count();
        return res;
    }

    public IEnumerable<User> Get()
    {
        var users = _db.GetCollection<User>().FindAll();
        return users;
    }

    public User? Get(string id)
    {
        var user = _db.GetCollection<User>().FindById(new ObjectId(id));
        return user ?? null;
    }

    public User? GetByName(string userName)
    {
        var user = _db.GetCollection<User>().FindOne(x => x.Login == userName);
        return user ?? null;
    }

    public User? Create(User user)
    {
        var id = _db.GetCollection<User>().Insert(user);
        var result = Get(id.AsObjectId.ToString());
        return result;
    }

    public bool Delete(string id)
    {
        var res = _db.GetCollection<User>().Delete(new ObjectId(id));
        return res;
    }

    public User? Authenticate(string username, string password)
    {
        var user = _db.GetCollection<User>().FindOne(x => x.Login == username && x.Password == password);
        return user ?? null;
    }

    public bool ChangePassword(string id, string password)
    {
        var user = _db.GetCollection<User>().FindById(new ObjectId(id))
            ?? throw new AfonyaErrorException("Пользователь не найден");
        user.SetPassword(password);
        _db.GetCollection<User>().Update(user);
        return true;
    }

    public bool Update(string id, string login, bool isAdmin)
    {
        var user = _db.GetCollection<User>().FindById(new ObjectId(id))
            ?? throw new AfonyaErrorException("Пользователь не найден");
        user.SetAdmin(isAdmin);
        user.SetLogin(login);
        _db.GetCollection<User>().Update(user);
        return true;
    }

}