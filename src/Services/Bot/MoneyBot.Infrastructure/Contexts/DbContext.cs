using LiteDB;
using MoneyBot.Domain.Entities;
using MoneyBot.Interfaces;

namespace MoneyBot.Infrastructure.Contexts;

public class DbContext : ILiteDbContext
{
    public LiteDatabase Database { get; }

    public DbContext(string connectionString)
    {
        var mapper = BsonMapper.Global;
        mapper.Entity<MoneyTransaction>().Id(x => x.Id);
        mapper.Entity<Category>().Id(x => x.Id);
        mapper.Entity<User>().Id(x => x.Id);
        Database = new LiteDatabase(connectionString, mapper);
    }
}