using Microsoft.AspNetCore.Mvc;

namespace Bot.Host.BasicAuth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BasicAuthAttribute : TypeFilterAttribute
{
    public BasicAuthAttribute(string realm = "App") : base(typeof(BasicAuthFilter))
    {
        Arguments = new object[] { realm };
    }
}