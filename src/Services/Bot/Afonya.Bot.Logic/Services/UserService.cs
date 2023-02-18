using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces;
using Afonya.Bot.Interfaces.Dto;
using Afonya.Bot.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly ILiteDbContext _db;
    private readonly AdminUser _admin;
    
    public UserService(ILogger<UserService> logger, ILiteDbContext db, IOptions<AdminUser> admin)
    {
        _logger = logger;
        _db = db;
        _admin = admin.Value;
    }

    public bool IsAdminUser(string userName, string password)
    {
        _logger.LogInformation("Validating admin user [{UserName}]", userName);
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) return false;
        var inputUser = new AdminUser(userName, password);
        return _admin == inputUser;
    }
    
    public bool TelegramUsersExists()
    {
        var res = _db.Database.GetCollection<TelegramUser>().Count() > 0;
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
        var user = _db.Database.GetCollection<TelegramUser>().FindById(id);
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
        var result = Get(id);
        return result;
    }

    public bool Delete(string id)
    {
        var res = _db.Database.GetCollection<TelegramUser>().Delete(id);
        return res;
    }
}