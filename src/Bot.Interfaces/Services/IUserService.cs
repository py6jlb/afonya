using Bot.Interfaces.Dto;

namespace Bot.Interfaces.Services;

public interface IUserService
{
    bool IsValidUser(string userName, string password);
    bool UsersExists();
    string AddUser(UserDto user);
}