using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.MoneyTransaction.Commands.CreateMoneyTransaction;

public class CreateMoneyTransactionCommand : IRequest<bool>
{
    public MoneyTransactionDto MoneyTransaction { get; set; }
}
