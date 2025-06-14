using Afonya.Domain.Repositories;
using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransaction;

public class GetMoneyTransactionQueryHandler : IRequestHandler<GetMoneyTransactionQuery, MoneyTransactionDto>
{

    private readonly IMoneyTransactionRepository _moneyTransactionRepository;

    public GetMoneyTransactionQueryHandler(IMoneyTransactionRepository moneyTransactionRepository)
    {
        _moneyTransactionRepository = moneyTransactionRepository;
    }

    public async Task<MoneyTransactionDto> Handle(GetMoneyTransactionQuery request, CancellationToken cancellationToken)
    {
        var t = _moneyTransactionRepository.Get(request.Id);
        var result = new MoneyTransactionDto
        {
            Id = t.Id?.ToString(),
            CategoryName = t.CategoryName,
            CategoryHumanName = t.CategoryHumanName,
            CategoryIcon = t.CategoryIcon,
            CategoryId = t.CategoryId,
            Value = t.Value,
            Sign = t.Sign,
            RegisterDate = t.RegisterDate,
            TransactionDate = t.TransactionDate,
            FromUserName = t.FromUserName
        };
        return result;
    }
}
