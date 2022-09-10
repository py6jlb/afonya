using LiteDB;
using Store.Contexts.Abstractions;
using Store.Dto;
using Store.Entities;
using Store.Services.Abstractions;

namespace Store.Services;

public class MoneyTransactionService : IMoneyTransactionService
{
    private readonly ILogger<MoneyTransactionService> _logger;
    private readonly LiteDatabase _db;
    public MoneyTransactionService(ILogger<MoneyTransactionService> logger, ILiteDbContext context)
    {
        _logger = logger;
        _db = context.Database;
    }

    public bool Delete(ObjectId id)
    {
        try
        {
            return _db.GetCollection<MoneyTransaction>().Delete(id);
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

        if (filter.IncludeDate)
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

    public MoneyTransaction Get(ObjectId id)
    {
        try
        {
            return _db.GetCollection<MoneyTransaction>().FindById(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка получения операции.");
            throw;
        }
    }

    public ObjectId Insert(MoneyTransaction moneyTransaction)
    {
        try
        {
            return _db.GetCollection<MoneyTransaction>().Insert(moneyTransaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка добавления операции.");
            throw;
        }
    }

    public bool Update(MoneyTransaction moneyTransaction)
    {
        try
        {
            return _db.GetCollection<MoneyTransaction>().Update(moneyTransaction);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка обновления операции.");
            throw;
        }
    }
}