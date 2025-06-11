using Afonya.Domain.Entities;
using LiteDB;

namespace Afonya.Infrastructure.Contexts;

public class DbContext
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