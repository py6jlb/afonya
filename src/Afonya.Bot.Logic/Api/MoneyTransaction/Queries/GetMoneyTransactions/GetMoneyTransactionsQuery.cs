using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.MoneyTransaction.Queries.GetMoneyTransactions;

public class GetMoneyTransactionsQuery : IRequest<IReadOnlyCollection<MoneyTransactionDto>>
{
    public int? Month { get; set; }
    public int? Year { get; set; }
    public string? User { get; set; }
    public string? Category { get; set; }
}