using Afonya.Domain.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransactions;

public class GetMoneyTransactionsQueryHandler : IRequestHandler<GetMoneyTransactionsQuery, IReadOnlyCollection<MoneyTransactionDto>>
{
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;

    public GetMoneyTransactionsQueryHandler(IMoneyTransactionRepository moneyTransactionRepository)
    {
        _moneyTransactionRepository = moneyTransactionRepository;
    }

    public Task<IReadOnlyCollection<MoneyTransactionDto>> Handle(GetMoneyTransactionsQuery request, CancellationToken cancellationToken)
    {
        var result = _moneyTransactionRepository.Get(request.Month, request.Year,
            request.User, request.Category).Select(x => new MoneyTransactionDto
            {
                Id = x.Id?.ToString(),
                CategoryName = x.CategoryName,
                CategoryHumanName = x.CategoryHumanName,
                CategoryIcon = x.CategoryIcon,
                Value = x.Value,
                Sign = x.Sign,
                RegisterDate = x.RegisterDate,
                TransactionDate = x.TransactionDate,
                FromUserName = x.FromUserName,
                Note = x.Note,
            }).ToArray();

        return Task.FromResult<IReadOnlyCollection<MoneyTransactionDto>>(result);
    }
}