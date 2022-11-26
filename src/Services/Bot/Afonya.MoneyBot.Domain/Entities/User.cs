using LiteDB;

namespace Afonya.MoneyBot.Domain.Entities;

public class User
{
    public ObjectId Id { get; set; }
    public string Login { get; set; }
}