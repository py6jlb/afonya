using Bot.Domain.Entities;
using Bot.Interfaces.Dto;
using LiteDB;

namespace Bot.Interfaces.Services;

public interface IMoneyTransactionService
{
    bool Delete(ObjectId id);
    IEnumerable<MoneyTransaction> Get(MoneyTransactionFilter filter);
    MoneyTransaction? Get(ObjectId id);
    ObjectId Insert(MoneyTransaction moneyTransaction);
    bool Update(MoneyTransaction moneyTransaction);
}