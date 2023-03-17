using Afonya.Bot.Interfaces.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.MoneyTransaction.Queries.GetMoneyTransactions;

public class GetMoneyTransactionsQueryHandler : IRequestHandler<GetMoneyTransactionsQuery, IReadOnlyCollection<MoneyTransactionDto>>
{
    private readonly IMoneyTransactionRepository _moneyTransactionRepository;

    public GetMoneyTransactionsQueryHandler(IMoneyTransactionRepository moneyTransactionRepository)
    {
        _moneyTransactionRepository = moneyTransactionRepository;
    }

    public Task<IReadOnlyCollection<MoneyTransactionDto>> Handle(GetMoneyTransactionsQuery request, CancellationToken cancellationToken)
    {
        var result = _moneyTransactionRepository.Get(request.Filter).Select(x => new MoneyTransactionDto
        {
            Id = x.Id?.ToString(),
            CategoryName = x.CategoryName,
            CategoryHumanName = x.CategoryHumanName,
            CategoryIcon = x.CategoryIcon,
            Value = x.Value,
            Sign = x.Sign,
            RegisterDate = x.RegisterDate,
            TransactionDate = x.TransactionDate,
            FromUserName = x.FromUserName
        }).ToArray();

        return Task.FromResult<IReadOnlyCollection<MoneyTransactionDto>>(result);
    }
}