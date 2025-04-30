using Afonya.Bot.Domain.Entities;

namespace Afonya.Bot.Domain.Repositories;

public interface IUserRepository
{
    int Count();
    IEnumerable<TelegramUser> Get();
    TelegramUser? Get(string id);
    TelegramUser? ChangePassword(string id, string password);
    TelegramUser? GetByName(string userName);
    TelegramUser? Create(TelegramUser user);
    TelegramUser? Authenticate(string username, string password);
    bool Delete(string id);
}