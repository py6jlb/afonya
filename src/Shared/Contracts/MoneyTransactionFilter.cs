namespace Shared.Contracts;

public class MoneyTransactionFilter
{
    public bool? IncludeDate { get; set; } = true;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? User { get; set; }
    public string? Category { get; set; }
}