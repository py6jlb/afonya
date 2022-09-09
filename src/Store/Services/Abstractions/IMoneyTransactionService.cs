using LiteDB;
using Store.Dto;
using Store.Entities;

namespace Store.Services.Abstractions;

public interface IMoneyTransactionService
{
    bool Delete(ObjectId id);
    IEnumerable<MoneyTransaction> Get(MoneyTransactionFilter filter);
    MoneyTransaction Get(ObjectId id);
    ObjectId Insert(MoneyTransaction moneyTransaction);
    bool Update(MoneyTransaction moneyTransaction);
}