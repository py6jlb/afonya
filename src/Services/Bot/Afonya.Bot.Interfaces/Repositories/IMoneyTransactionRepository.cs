using Afonya.Bot.Domain.Entities;
using Shared.Contracts;

namespace Afonya.Bot.Interfaces.Repositories;

public interface IMoneyTransactionRepository
{
    bool Delete(string id);
    IEnumerable<MoneyTransaction> Get(MoneyTransactionFilter filter);
    MoneyTransaction? Get(string id);
    string Insert(MoneyTransaction moneyTransaction);
    bool Update(MoneyTransaction moneyTransaction);
}