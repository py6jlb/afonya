using Afonya.Bot.Domain.Enums;
using LiteDB;

namespace Afonya.Bot.Domain.Entities;

public class Callback
{
    public ObjectId Id { get; set; }
    public CallbackCommand Command { get; set; }
    public Guid? GroupId { get; set; }
    public string JsonData { get; set; }
}