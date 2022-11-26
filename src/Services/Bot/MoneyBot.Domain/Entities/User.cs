using LiteDB;

namespace MoneyBot.Domain.Entities;

public class User
{
    public ObjectId Id { get; set; }
    public string Login { get; set; }
}