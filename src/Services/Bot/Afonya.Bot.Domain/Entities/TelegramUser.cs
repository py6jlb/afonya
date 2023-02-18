using LiteDB;

namespace Afonya.Bot.Domain.Entities;

public class TelegramUser
{
    public ObjectId Id { get; set; }
    public string Login { get; set; }
}
