using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces;
using LiteDB;

namespace Afonya.Bot.Infrastructure.Contexts;

public class DbContext : ILiteDbContext
{
    public LiteDatabase Database { get; }

    public DbContext(string connectionString)
    {
        var mapper = BsonMapper.Global;
        mapper.Entity<MoneyTransaction>().Id(x => x.Id);
        mapper.Entity<Category>().Id(x => x.Id);
        mapper.Entity<TelegramUser>().Id(x => x.Id);
        Database = new LiteDatabase(connectionString, mapper);
    }
}