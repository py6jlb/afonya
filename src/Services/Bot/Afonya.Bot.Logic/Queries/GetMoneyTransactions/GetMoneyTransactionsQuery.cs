using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Queries.GetMoneyTransactions;

public class GetMoneyTransactionsQuery : IRequest<IReadOnlyCollection<MoneyTransactionDto>>
{
    public MoneyTransactionFilter Filter { get; set; }
}