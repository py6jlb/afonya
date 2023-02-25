using System.Collections.ObjectModel;
using Afonya.Bot.Interfaces.Services;
using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Queries.GetMoneyTransactions;

public class GetMoneyTransactionsQueryHandler : IRequestHandler<GetMoneyTransactionsQuery, IReadOnlyCollection<MoneyTransactionDto>>
{
    private readonly IMoneyTransactionService _moneyTransactionService;

    public GetMoneyTransactionsQueryHandler(IMoneyTransactionService moneyTransactionService)
    {
        _moneyTransactionService = moneyTransactionService;
    }

    public Task<IReadOnlyCollection<MoneyTransactionDto>> Handle(GetMoneyTransactionsQuery request, CancellationToken cancellationToken)
    {
        var result = _moneyTransactionService.Get(request.Filter).Select(x => new MoneyTransactionDto
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