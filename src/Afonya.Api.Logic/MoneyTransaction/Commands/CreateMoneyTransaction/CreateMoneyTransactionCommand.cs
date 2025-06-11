using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.MoneyTransaction.Commands.CreateMoneyTransaction;

public class CreateMoneyTransactionCommand : IRequest<bool>
{
    public MoneyTransactionDto MoneyTransaction { get; set; }
}
