using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Infrastructure.Contexts;
using Afonya.Bot.Interfaces.Repositories;
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

    public IEnumerable<MoneyTransaction> Get(MoneyTransactionFilter filter)
    {
        var query = _db.GetCollection<MoneyTransaction>().Query();

        filter.StartDate ??= DateTime.MinValue;
        filter.EndDate ??= DateTime.MaxValue;

        if (filter.IncludeDate ?? true)
            query.Where(x => x.RegisterDate >= filter.StartDate && x.RegisterDate <= filter.EndDate);
        else
            query.Where(x => x.RegisterDate > filter.StartDate && x.RegisterDate < filter.EndDate);

        if (string.IsNullOrWhiteSpace(filter.Category))
            query.Where(x => x.CategoryName.Equals(filter.Category, StringComparison.InvariantCultureIgnoreCase));

        if (string.IsNullOrWhiteSpace(filter.User))
            query.Where(x => x.FromUserName.Equals(filter.User, StringComparison.InvariantCultureIgnoreCase));

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