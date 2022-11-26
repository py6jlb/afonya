using Shared.Contracts;

namespace Afonya.MoneyBot.Interfaces.Services;

public interface IMoneyTransactionService
{
    bool Delete(string id);
    IEnumerable<MoneyTransactionDto> Get(MoneyTransactionFilter filter);
    MoneyTransactionDto? Get(string id);
    string Insert(MoneyTransactionDto moneyTransaction);
    bool Update(MoneyTransactionDto moneyTransaction);
}