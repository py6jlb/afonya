using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Domain.Repositories;
using Afonya.Bot.Infrastructure.Contexts;
using LiteDB;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace Afonya.Bot.Infrastructure.Repositories;

public class MoneyTransactionRepository : IMoneyTransactionRepository
{
    private readonly ILogger<MoneyTransactionRepository> _logger;
    private readonly LiteDatabase _db;
    public MoneyTransactionRepository(ILogger<MoneyTransactionRepository> logger, DbContext context)
    {
        _logger = logger;
        _db = context.Database;
    }

    public bool Delete(string id)
    {
        try
        {
            var objectId = new ObjectId(id);
            return _db.GetCollection<MoneyTransaction>().Delete(objectId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка удаления операции.");
            throw;
        }
    }

    public IEnumerable<MoneyTransaction> Get(DateTime? start, DateTime? end, bool? includeDate, string? user, string? category)
    {
        var query = _db.GetCollection<MoneyTransaction>().Query();

        start ??= DateTime.MinValue;
        end ??= DateTime.MaxValue;

        if (includeDate ?? true)
            query.Where(x => x.RegisterDate >= start && x.RegisterDate <=end);
        else
            query.Where(x => x.RegisterDate > start && x.RegisterDate < end);

        if (string.IsNullOrWhiteSpace(category))
            query.Where(x => x.CategoryName.Equals(category, StringComparison.InvariantCultureIgnoreCase));

        if (string.IsNullOrWhiteSpace(user))
            query.Where(x => x.FromUserName.Equals(user, StringComparison.InvariantCultureIgnoreCase));

        var result = query.OrderBy(x => x.RegisterDate).ToEnumerable();
        return result;
    }

    public MoneyTransaction Get(string id)
    {
        try
        {
            var objectId = new ObjectId(id);
            var result =  _db.GetCollection<MoneyTransaction>().FindById(new ObjectId(objectId));
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка получения.");
            throw;
        }
    }

    public string Insert(MoneyTransaction moneyTransaction)
    {
        try
        {
            var id = _db.GetCollection<MoneyTransaction>().Insert(moneyTransaction);
            return id.AsObjectId.ToString();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка добавления.");
            throw;
        }
    }

    public bool Update(MoneyTransaction update)
    {
        try
        {
            return _db.GetCollection<MoneyTransaction>().Update(update);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка обновления.");
            throw;
        }
    }
}