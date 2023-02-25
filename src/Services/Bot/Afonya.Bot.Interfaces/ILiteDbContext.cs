using LiteDB;

namespace Afonya.Bot.Interfaces;

public interface ILiteDbContext
{
    LiteDatabase Database { get; }
}