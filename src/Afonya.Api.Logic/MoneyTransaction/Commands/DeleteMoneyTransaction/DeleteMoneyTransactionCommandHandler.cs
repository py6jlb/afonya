using System;
using Afonya.Domain.Repositories;
using MediatR;

namespace Afonya.Api.Logic.MoneyTransaction.Commands.DeleteMoneyTransaction;

public class DeleteMoneyTransactionCommandHandler : IRequestHandler<DeleteMoneyTransactionCommand, bool>
{
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;

    public DeleteMoneyTransactionCommandHandler(IMoneyTransactionRepository moneyTransactionRepository)
    {
        _moneyTransactionRepository = moneyTransactionRepository;
    }

    public Task<bool> Handle(DeleteMoneyTransactionCommand request, CancellationToken cancellationToken)
    {
        var result = _moneyTransactionRepository.Delete(request.Id);
        return Task.FromResult(result);
    }
}
