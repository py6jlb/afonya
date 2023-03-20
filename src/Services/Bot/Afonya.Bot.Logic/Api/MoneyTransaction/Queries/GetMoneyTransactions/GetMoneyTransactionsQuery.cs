using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.MoneyTransaction.Queries.GetMoneyTransactions;

public class GetMoneyTransactionsQuery : IRequest<IReadOnlyCollection<MoneyTransactionDto>>
{
    public bool? IncludeDate { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? User { get; set; }
    public string? Category { get; set; }
}