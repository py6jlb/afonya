using MoneyBot.Interfaces.Dto;

namespace MoneyBot.Interfaces.Services;

public interface IUserService
{
    bool UsersExists();
    IReadOnlyCollection<UserDto> Get();
    UserDto? Get(string id);
    UserDto? Create(UserDto user);
    bool Delete(string id);
}