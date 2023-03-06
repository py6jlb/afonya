namespace Afonya.Bot.Domain.Entities;

public class MoneyTransaction : BaseEntity
{
    protected MoneyTransaction(){}

    public MoneyTransaction(float value, int messageId, long chatId, string sign, string? categoryName, string? categoryHumanName, string? categoryIcon, DateTime registerDate, DateTime? transactionDate, string fromUserName)
    {
        Value = value;
        MessageId = messageId;
        ChatId = chatId;
        Sign = sign;
        CategoryName = categoryName;
        CategoryHumanName = categoryHumanName;
        CategoryIcon = categoryIcon;
        RegisterDate = registerDate;
        TransactionDate = transactionDate;
        FromUserName = fromUserName;
    }

    public float Value { get; private set; }
    public int MessageId { get; private set; }
    public long ChatId { get; private set; }
    public string Sign { get; private set; }
    public string CategoryName { get; private set; }
    public string CategoryHumanName { get; private set; }
    public string CategoryIcon { get; private set; }
    public DateTime RegisterDate { get; private set; }
    public DateTime? TransactionDate { get; private set; }
    public string FromUserName { get; private set; }


    public void SetValue( float value)
    {
        Value = value;
    }

    public void SetMessageId(int messageId)
    {
        MessageId = messageId;
    }

    public void SetChatId(long chatId)
    {
        ChatId = chatId;
    }

    public void SetSign(string sign)
    {
        Sign = sign;
    }

    public void SetCategory(string name, string icon, string humanName)
    {
        CategoryName = name;
        CategoryIcon = icon;
        CategoryHumanName = humanName;
    }

    public void SetUser(string from)
    {
        FromUserName = from;
    }

    public void SetTransactionDate(DateTime? date)
    {
        TransactionDate = date;
    }

    public void SetRegisterDate(DateTime date)
    {
        RegisterDate = date;
    }
}