using LiteDB;

namespace Bot.Domain.Entities;

public class User
{
    public ObjectId Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}