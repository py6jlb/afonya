using LiteDB;

namespace Afonya.MoneyBot.Interfaces;

public interface ILiteDbContext
{
    LiteDatabase Database { get; }
}