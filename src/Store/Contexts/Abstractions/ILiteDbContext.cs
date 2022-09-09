using LiteDB;

namespace Store.Contexts.Abstractions;

public interface ILiteDbContext
{
    LiteDatabase Database { get; }
}