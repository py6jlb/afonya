using Bot.Domain.Entities;
using Bot.Interfaces;
using Bot.Interfaces.Dto;
using Bot.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Bot.Logic.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly ILiteDbContext _db;
    
    public UserService(ILogger<UserService> logger, ILiteDbContext db)
    {
        _logger = logger;
        _db = db;
    }

    public bool IsValidUser(string userName, string password)
    {
        _logger.LogInformation("Validating user [{UserName}]", userName);
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)) return false;
        var user = _db.Database.GetCollection<User>().FindOne(x => x.Login == userName && x.Password == password);
        return user != null;
    }

    public bool UsersExists()
    {
        var res = _db.Database.GetCollection<User>().Count() > 0;
        return res;
    }

    public string AddUser(UserDto user)
    {
        var entity = new User
        {
            Login = user.Login,
            Password = user.Password
        };

        var res = _db.Database.GetCollection<User>().Insert(entity);
        return res.ToString();
    }
}