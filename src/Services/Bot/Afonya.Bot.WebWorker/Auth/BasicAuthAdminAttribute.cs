using Microsoft.AspNetCore.Mvc;

namespace Afonya.Bot.WebWorker.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BasicAuthAdminAttribute : TypeFilterAttribute
{
    public BasicAuthAdminAttribute(string realm = "Afonya") : base(typeof(BasicAuthAdminFilter))
    {
        Arguments = new object[] { realm };
    }
}