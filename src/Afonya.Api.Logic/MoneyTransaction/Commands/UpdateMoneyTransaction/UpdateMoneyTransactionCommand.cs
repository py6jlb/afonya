using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.MoneyTransaction.Commands.UpdateMoneyTransaction;

public class UpdateMoneyTransactionCommand : IRequest<bool>
{
    public MoneyTransactionDto MoneyTransaction { get; set; }
}