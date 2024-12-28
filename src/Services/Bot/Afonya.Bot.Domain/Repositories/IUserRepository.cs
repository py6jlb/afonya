using Afonya.Bot.Domain.Entities;

namespace Afonya.Bot.Domain.Repositories;

public interface IUserRepository
{
    int Count();
    IEnumerable<TelegramUser> Get();
    TelegramUser? Get(string id);
    TelegramUser? GetByName(string userName);
    TelegramUser? Create(TelegramUser user);
    bool Delete(string id);
}