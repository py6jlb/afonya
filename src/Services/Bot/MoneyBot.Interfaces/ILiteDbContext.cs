using LiteDB;

namespace MoneyBot.Interfaces;

public interface ILiteDbContext
{
    LiteDatabase Database { get; }
}