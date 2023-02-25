using Shared.Contracts;

namespace Afonya.Bot.Interfaces.Services;

public interface IUserService
{
    bool IsAdminUser(string userName, string password);
    bool TelegramUsersExists();
    IReadOnlyCollection<UserDto> Get();
    UserDto? Get(string id);
    UserDto? GetByName(string userName);
    UserDto? Create(UserDto user);
    bool Delete(string id);
}