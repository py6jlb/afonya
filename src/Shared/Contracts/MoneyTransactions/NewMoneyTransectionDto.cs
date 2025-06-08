namespace Shared.Contracts;

public class NewMoneyTransactionDto
{
    public float Value { get; set; }
    public string Sign { get; set; }
    public string CategoryId { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string FromUserName { get; set; }
}
