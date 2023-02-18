﻿using Microsoft.AspNetCore.Mvc;

namespace Afonya.MoneyBot.WebWorker.Auth;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BasicAuthAttribute : TypeFilterAttribute
{
    public BasicAuthAttribute(string realm = "Afonya") : base(typeof(BasicAuthFilter))
    {
        Arguments = new object[] { realm };
    }
}