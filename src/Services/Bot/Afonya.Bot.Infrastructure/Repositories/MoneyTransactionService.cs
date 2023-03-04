using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Interfaces;
using Afonya.Bot.Interfaces.Repositories;
using LiteDB;
using Microsoft.Extensions.Logging;
using Shared.Contracts;

namespace Afonya.Bot.Infrastructure.Repositories;

public class MoneyTransactionRepository : IMoneyTransactionRepository
{
    private readonly ILogger<MoneyTransactionRepository> _logger;
    private readonly LiteDatabase _db;
    public MoneyTransactionRepository(ILogger<MoneyTransactionRepository> logger, ILiteDbContext context)
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

    public IReadOnlyCollection<MoneyTransactionDto> Get(MoneyTransactionFilter filter)
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
        return result.Select(x=> new MoneyTransactionDto
        {
            Id = x.Id.ToString(), 
            CategoryName = x.CategoryName, 
            CategoryHumanName = x.CategoryHumanName, 
            CategoryIcon = x.CategoryIcon, 
            Value = x.Value, 
            Sign = x.Sign, 
            RegisterDate = x.RegisterDate, 
            TransactionDate = x.TransactionDate, 
            FromUserName = x.FromUserName
        }).ToArray();
    }

    public MoneyTransactionDto Get(string id)
    {
        try
        {
            var objectId = new ObjectId(id);
            var res =  _db.GetCollection<MoneyTransaction>().FindById(new ObjectId(objectId));
            return new MoneyTransactionDto
            {
                Id = res.Id.ToString(), 
                CategoryName = res.CategoryName, 
                CategoryHumanName = res.CategoryHumanName, 
                CategoryIcon = res.CategoryIcon, 
                Value = res.Value, 
                Sign = res.Sign, 
                RegisterDate = res.RegisterDate, 
                TransactionDate = res.TransactionDate, 
                FromUserName = res.FromUserName
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка получения.");
            throw;
        }
    }

    public string Insert(MoneyTransactionDto moneyTransaction)
    {
        try
        {
            var entity = new MoneyTransaction
            {
                CategoryName = moneyTransaction.CategoryName, 
                CategoryHumanName = moneyTransaction.CategoryHumanName, 
                CategoryIcon = moneyTransaction.CategoryIcon, 
                Value = moneyTransaction.Value, 
                Sign = moneyTransaction.Sign, 
                RegisterDate = moneyTransaction.RegisterDate, 
                TransactionDate = moneyTransaction.TransactionDate, 
                FromUserName = moneyTransaction.FromUserName,
                MessageId = moneyTransaction.MessageId,
                ChatId = moneyTransaction.ChatId
            };
            var id = _db.GetCollection<MoneyTransaction>().Insert(entity);
            return id.AsObjectId.ToString();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка добавления.");
            throw;
        }
    }

    public bool Update(MoneyTransactionDto moneyTransaction)
    {
        try
        {
            var entity = new MoneyTransaction
            {
                Id = new ObjectId(moneyTransaction.Id),
                CategoryName = moneyTransaction.CategoryName, 
                CategoryHumanName = moneyTransaction.CategoryHumanName, 
                CategoryIcon = moneyTransaction.CategoryIcon, 
                Value = moneyTransaction.Value, 
                Sign = moneyTransaction.Sign, 
                RegisterDate = moneyTransaction.RegisterDate, 
                TransactionDate = moneyTransaction.TransactionDate, 
                FromUserName = moneyTransaction.FromUserName,
                MessageId = moneyTransaction.MessageId,
                ChatId = moneyTransaction.ChatId
            };
            return _db.GetCollection<MoneyTransaction>().Update(entity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка обновления.");
            throw;
        }
    }
}