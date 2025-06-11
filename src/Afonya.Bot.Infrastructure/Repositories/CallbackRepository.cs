using Afonya.Bot.Domain.Entities;
using Afonya.Bot.Domain.Repositories;
using Afonya.Bot.Infrastructure.Contexts;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Afonya.Bot.Infrastructure.Repositories;

public class CallbackRepository : ICallbackRepository
{
    private readonly ILogger<CallbackRepository> _logger;
    private readonly LiteDatabase _db;

    public CallbackRepository(ILogger<CallbackRepository> logger, DbContext context)
    {
        _logger = logger;
        _db = context.Database;
    }

    public IReadOnlyCollection<Callback>? GetGroup(Guid groupId)
    {
        var res = _db.GetCollection<Callback>().Find(x => x.GroupId == groupId);
        return res.ToArray();
    }

    public Callback? Get(string id)
    {
        var cat = _db.GetCollection<Callback>().FindById(new ObjectId(id));
        return cat;
    }

    public Callback Create(Callback callback)
    {
        var res = _db.GetCollection<Callback>().Insert(callback);
        var newCallback = Get(res.AsObjectId.ToString());
        return newCallback;
    }

    public bool Delete(string id)
    {
        var result = _db.GetCollection<Callback>().Delete(new ObjectId(id));
        return result;
    }
}