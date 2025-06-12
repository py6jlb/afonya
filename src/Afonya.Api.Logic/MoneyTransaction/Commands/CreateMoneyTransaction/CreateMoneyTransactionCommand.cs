using MediatR;

namespace Afonya.Api.Logic.MoneyTransaction.Commands.CreateMoneyTransaction;

public class CreateMoneyTransactionCommand : IRequest<bool>
{
    public string Sign { get; set; }
    public float Value { get; set; }
    public DateTime Date { get; set; }
    public string? CategoryId { get; set; }
    public string FromUsername { get; set; }
}
