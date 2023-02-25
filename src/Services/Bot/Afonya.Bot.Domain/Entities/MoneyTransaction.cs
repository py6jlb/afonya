using LiteDB;

namespace Afonya.Bot.Domain.Entities;

public class MoneyTransaction
{
    public ObjectId Id { get; set; }
    public float Value { get; set; }
    public int MessageId { get; set; }
    public long ChatId { get; set; }
    public string Sign { get; set; }
    public string CategoryName { get; set; }
    public string CategoryHumanName { get; set; }
    public string CategoryIcon { get; set; }
    public DateTime RegisterDate { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string FromUserName { get; set; }
}