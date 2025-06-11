using Afonya.Bot.Domain.Entities;

namespace Afonya.Bot.Domain.Repositories;

public interface IMoneyTransactionRepository
{
    bool Delete(string id);
    IEnumerable<MoneyTransaction> Get(int? month, int? year, string? user, string? category);
    MoneyTransaction? Get(string id);
    string Insert(MoneyTransaction moneyTransaction);
    bool Update(MoneyTransaction moneyTransaction);
}