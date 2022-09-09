using LiteDB;
using Microsoft.Extensions.Options;
using Store.Contexts.Abstractions;
using Store.Dto;
using Store.Entities;

namespace Store.Contexts;

public class DbContext : ILiteDbContext
{
    public LiteDatabase Database { get; }

    public DbContext(IOptions<DbOptions> options)
    {
        var mapper = BsonMapper.Global;
        mapper.Entity<MoneyTransaction>().Id(x => x.Id);
        Database = new LiteDatabase(options.Value.ConnectionString, mapper);
    }
}