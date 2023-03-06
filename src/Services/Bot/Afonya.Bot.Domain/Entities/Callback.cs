using Afonya.Bot.Domain.Enums;

namespace Afonya.Bot.Domain.Entities;

public class Callback : BaseEntity
{
    protected Callback(){}

    public Callback(CallbackCommand command, Guid? groupId, string jsonData)
    {
        Command = command;
        GroupId = groupId;
        JsonData = jsonData;
    }

    public CallbackCommand Command { get; private set; }
    public Guid? GroupId { get; private set; }
    public string JsonData { get; private set; }

    public void SetCallbackCommandType(CallbackCommand type)
    {
        Command = type;
    }

    public void SetGroupId(Guid groupId)
    {
        GroupId = groupId;
    }

    public void SetJsonData(string json)
    {
        JsonData = json;
    }
}