using Afonya.Domain.Exceptions;
using Afonya.Domain.Repositories;
using MediatR;

namespace Afonya.Api.Logic.MoneyTransaction.Commands.UpdateMoneyTransaction;

public class UpdateMoneyTransactionCommandHandler : IRequestHandler<UpdateMoneyTransactionCommand, bool>
{
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;
    private readonly ICategoryRepository _categoryRepository;

    public UpdateMoneyTransactionCommandHandler(IMoneyTransactionRepository moneyTransactionRepository, ICategoryRepository categoryRepository)
    {
        _moneyTransactionRepository = moneyTransactionRepository;
        _categoryRepository = categoryRepository;
    }

    public Task<bool> Handle(UpdateMoneyTransactionCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
            throw new AfonyaErrorException("Отсутствует id записи для обновления.");

        var category = _categoryRepository.Get(request.CategoryId) ?? throw new AfonyaErrorException("Отсутствует категория для записи.");

        var entity = _moneyTransactionRepository.Get(request.Id) ?? throw new AfonyaErrorException("Транзакция для обновления не найдена");
        entity.SetValue(request.Value);
        entity.SetSign(request.Sign);
        entity.SetCategory(category.Id.ToString(), category.Name,
            category.Icon,
            category.HumanName);
        entity.SetTransactionDate(request.TransactionDate);
        entity.SetUser(request.FromUserName);
        entity.SetNote(request.Note);

        var res = _moneyTransactionRepository.Update(entity);
        return Task.FromResult(res);
    }
}