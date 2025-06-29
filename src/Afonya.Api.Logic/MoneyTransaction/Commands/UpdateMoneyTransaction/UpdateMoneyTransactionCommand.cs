using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.MoneyTransaction.Commands.UpdateMoneyTransaction;

public class UpdateMoneyTransactionCommand : IRequest<bool>
{
    public string Id { get; set; }
    public float Value { get; set; }
    public string Sign { get; set; }
    public string CategoryId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string FromUserName { get; set; }
    public string Note { get; set; }
}