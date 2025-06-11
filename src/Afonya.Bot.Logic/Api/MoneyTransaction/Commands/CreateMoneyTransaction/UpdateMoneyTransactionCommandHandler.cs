using System;
using MediatR;

namespace Afonya.Bot.Logic.Api.MoneyTransaction.Commands.CreateMoneyTransaction;

public class UpdateMoneyTransactionCommandHandler : IRequestHandler<CreateMoneyTransactionCommand, bool>
{
    public Task<bool> Handle(CreateMoneyTransactionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
