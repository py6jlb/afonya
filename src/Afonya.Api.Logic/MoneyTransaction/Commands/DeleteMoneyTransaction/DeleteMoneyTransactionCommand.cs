using System;
using MediatR;

namespace Afonya.Api.Logic.MoneyTransaction.Commands.DeleteMoneyTransaction;

public class DeleteMoneyTransactionCommand: IRequest<bool>
{
    public string Id { get; set; }
}
