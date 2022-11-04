using LiteDB;

namespace Bot.Interfaces;

public interface ILiteDbContext
{
    LiteDatabase Database { get; }
}