using Shared.Contracts;

namespace Afonya.Bot.Interfaces.Repositories;

public interface IUserRepository
{
    int Count();
    IReadOnlyCollection<UserDto> Get();
    UserDto? Get(string id);
    UserDto? GetByName(string userName);
    UserDto? Create(UserDto user);
    bool Delete(string id);
}