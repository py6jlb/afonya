namespace Afonya.Domain.Entities;

public class MoneyTransaction : BaseEntity
{
    protected MoneyTransaction() { }

    public MoneyTransaction(float value, int messageId, long chatId,
        string sign, string? categoryId, string? categoryName, string? categoryHumanName,
        string? categoryIcon, DateTime registerDate, DateTime? transactionDate, string fromUserName, string? note)
    {
        Value = value;
        MessageId = messageId;
        ChatId = chatId;
        Sign = sign;
        CategoryId = categoryId;
        CategoryName = categoryName;
        CategoryHumanName = categoryHumanName;
        CategoryIcon = categoryIcon;
        RegisterDate = registerDate;
        TransactionDate = transactionDate;
        FromUserName = fromUserName;
        Note = note;
    }

    public MoneyTransaction(float value, string sign, string? categoryId,
        string? categoryName, string? categoryHumanName, string? categoryIcon,
        DateTime registerDate, DateTime? transactionDate, string fromUserName, string? note)
    {
        Value = value;
        Sign = sign;
        CategoryId = categoryId;
        CategoryName = categoryName;
        CategoryHumanName = categoryHumanName;
        CategoryIcon = categoryIcon;
        RegisterDate = registerDate;
        TransactionDate = transactionDate;
        FromUserName = fromUserName;
        Note = note;
    }

    public float Value { get; private set; }
    public int? MessageId { get; private set; }
    public long? ChatId { get; private set; }
    public string Sign { get; private set; }
    public string CategoryId { get; private set; }
    public string CategoryName { get; private set; }
    public string CategoryHumanName { get; private set; }
    public string CategoryIcon { get; private set; }
    public DateTime RegisterDate { get; private set; }
    public DateTime? TransactionDate { get; private set; }
    public string FromUserName { get; private set; }
    public string Note { get; private set; }


    public void SetValue(float value)
    {
        Value = value;
    }

    //public void SetMessageId(int messageId)
    //{
    //    MessageId = messageId;
    //}

    //public void SetChatId(long chatId)
    //{
    //    ChatId = chatId;
    //}

    public void SetSign(string sign)
    {
        Sign = sign;
    }

    public void SetCategory(string id, string name, string icon, string humanName)
    {
        CategoryId = id;
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

    public void SetNote(string note)
    {
        Note = note;
    }
}