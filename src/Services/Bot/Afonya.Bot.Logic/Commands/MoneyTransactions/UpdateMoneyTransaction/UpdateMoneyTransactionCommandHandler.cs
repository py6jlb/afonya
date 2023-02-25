using Afonya.Bot.Interfaces.Services;
using Common.Exceptions;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.MoneyTransactions.UpdateMoneyTransaction;

public class UpdateMoneyTransactionCommandHandler : IRequestHandler<UpdateMoneyTransactionCommand, bool>
{
    private readonly IMoneyTransactionService _moneyTransactionService;

    public UpdateMoneyTransactionCommandHandler(IMoneyTransactionService moneyTransactionService)
    {
        _moneyTransactionService = moneyTransactionService;
    }

    public Task<bool> Handle(UpdateMoneyTransactionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.MoneyTransaction.Id))
            throw new AfonyaErrorException("Отсуствует id записи для обновления.");

        var entity = _moneyTransactionService.Get(request.MoneyTransaction.Id);
        if (entity == null)
            throw new AfonyaErrorException("Транзакция для обновления не найдена");

        entity.Value = request.MoneyTransaction.Value;
        entity.Sign = request.MoneyTransaction.Sign;
        entity.CategoryName = request.MoneyTransaction.CategoryName;
        entity.CategoryHumanName = request.MoneyTransaction.CategoryHumanName;
        entity.CategoryIcon = request.MoneyTransaction.CategoryIcon;
        entity.RegisterDate = request.MoneyTransaction.RegisterDate;
        entity.TransactionDate = request.MoneyTransaction.TransactionDate;
        entity.FromUserName = request.MoneyTransaction.FromUserName;

        var res = _moneyTransactionService.Update(entity);
        return Task.FromResult(res);
    }
}