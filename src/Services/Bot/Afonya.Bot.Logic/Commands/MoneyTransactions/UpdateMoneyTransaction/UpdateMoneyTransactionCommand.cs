using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.MoneyTransactions.UpdateMoneyTransaction;

public class UpdateMoneyTransactionCommand : IRequest<bool>
{
    public MoneyTransactionDto MoneyTransaction { get; set; }
}