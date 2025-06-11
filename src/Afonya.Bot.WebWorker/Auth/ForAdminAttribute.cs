namespace Afonya.Bot.WebWorker.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ForAdminAttribute : Attribute
{
    public override string ToString()
    {
        return "ForAdmin";
    }
}