using System;
using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransaction;

public class GetMoneyTransactionQuery : IRequest<MoneyTransactionDto>
{
    public string Id { get; set; }
}
