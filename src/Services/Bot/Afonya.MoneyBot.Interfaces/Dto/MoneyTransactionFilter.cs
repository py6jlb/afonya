namespace Afonya.MoneyBot.Interfaces.Dto;

public class MoneyTransactionFilter
{
    public bool IncludeDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? User { get; set; }
    public string? Category { get; set; }
}