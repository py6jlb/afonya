using Afonya.Bot.Domain.Entities;

namespace Afonya.Bot.Interfaces.Repositories;

public interface IUserRepository
{
    int Count();
    IEnumerable<TelegramUser> Get();
    TelegramUser? Get(string id);
    TelegramUser? GetByName(string userName);
    TelegramUser? Create(TelegramUser user);
    bool Delete(string id);
}