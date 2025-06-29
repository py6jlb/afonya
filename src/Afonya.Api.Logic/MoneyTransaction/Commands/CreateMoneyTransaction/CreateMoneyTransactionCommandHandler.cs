using System;
using Afonya.Domain.Repositories;
using MediatR;
using Afonya.Domain.Entities;

namespace Afonya.Api.Logic.MoneyTransaction.Commands.CreateMoneyTransaction;

public class CreateMoneyTransactionCommandHandler : IRequestHandler<CreateMoneyTransactionCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;

    public CreateMoneyTransactionCommandHandler(ICategoryRepository categoryRepository, IMoneyTransactionRepository moneyTransactionRepository)
    {
        _categoryRepository = categoryRepository;
        _moneyTransactionRepository = moneyTransactionRepository;
    }

    public Task<bool> Handle(CreateMoneyTransactionCommand request, CancellationToken cancellationToken)
    {
        var category = _categoryRepository.Get(request.CategoryId) ?? new Category("", "unknown", "unknown", true);
        var newTransaction = new Domain.Entities.MoneyTransaction(
            request.Value,
            request.Sign,
            category.Id.ToString(),
            category.Name,
            category.HumanName,
            category.Icon,
            DateTime.Now,
            request.Date,
            request.FromUsername,
            request.Note);

        _moneyTransactionRepository.Insert(newTransaction);
        return Task.FromResult(true);
    }
}
