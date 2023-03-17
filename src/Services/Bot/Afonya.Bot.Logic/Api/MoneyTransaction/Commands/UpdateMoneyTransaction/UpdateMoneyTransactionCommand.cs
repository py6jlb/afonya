using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.MoneyTransaction.Commands.UpdateMoneyTransaction;

public class UpdateMoneyTransactionCommand : IRequest<bool>
{
    public MoneyTransactionDto MoneyTransaction { get; set; }
}