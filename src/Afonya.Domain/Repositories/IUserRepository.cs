using Afonya.Domain.Entities;

namespace Afonya.Domain.Repositories;

public interface IUserRepository
{
    int Count();
    IEnumerable<User> Get();
    User? Get(string id);
    User? ChangePassword(string id, string password);
    User? GetByName(string userName);
    User? Create(User user);
    User? Authenticate(string username, string password);
    bool Delete(string id);
}