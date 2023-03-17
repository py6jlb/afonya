using Afonya.Bot.Domain.Exceptions;
using Afonya.Bot.Interfaces.Repositories;
using MediatR;

namespace Afonya.Bot.Logic.Api.MoneyTransaction.Commands.UpdateMoneyTransaction;

public class UpdateMoneyTransactionCommandHandler : IRequestHandler<UpdateMoneyTransactionCommand, bool>
{
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;

    public UpdateMoneyTransactionCommandHandler(IMoneyTransactionRepository moneyTransactionRepository)
    {
        _moneyTransactionRepository = moneyTransactionRepository;
    }

    public Task<bool> Handle(UpdateMoneyTransactionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.MoneyTransaction.Id))
            throw new AfonyaErrorException("Отсуствует id записи для обновления.");

        var entity = _moneyTransactionRepository.Get(request.MoneyTransaction.Id);
        if (entity == null)
            throw new AfonyaErrorException("Транзакция для обновления не найдена");

        entity.SetValue(request.MoneyTransaction.Value);
        entity.SetSign(request.MoneyTransaction.Sign);
        entity.SetCategory(request.MoneyTransaction.CategoryName, 
            request.MoneyTransaction.CategoryIcon,
            request.MoneyTransaction.CategoryHumanName);
        entity.SetRegisterDate(request.MoneyTransaction.RegisterDate);
        entity.SetTransactionDate(request.MoneyTransaction.TransactionDate);
        entity.SetUser(request.MoneyTransaction.FromUserName);

        var res = _moneyTransactionRepository.Update(entity);
        return Task.FromResult(res);
    }
}