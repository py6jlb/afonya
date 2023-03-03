using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Afonya.Bot.Interfaces.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Afonya.Bot.WebWorker.Auth;

public class BasicAuthAdminFilter : IAuthorizationFilter
{
    private readonly string _realm;

    public BasicAuthAdminFilter(string realm)
    {
        _realm = realm;
        if (string.IsNullOrWhiteSpace(_realm))
        {
            throw new ArgumentNullException(nameof(realm), @"Please provide a non-empty realm value.");
        }
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            if (authHeader == null) ReturnUnauthorizedResult(context);
            var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
            if (!authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
                ReturnUnauthorizedResult(context);
            
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty))
                .Split(':', 2);
            if (credentials.Length == 2 && IsAuthorized(context, credentials[0], credentials[1])) return;
            ReturnUnauthorizedResult(context);
        }
        catch (FormatException)
        {
            ReturnUnauthorizedResult(context);
        }
    }

    private bool IsAuthorized(AuthorizationFilterContext context, string username, string password)
    {
        var adminUser = context.HttpContext.RequestServices.GetRequiredService<IOptions<AdminUser>>().Value;
        return adminUser.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) 
               && adminUser.Password.Equals(password, StringComparison.InvariantCultureIgnoreCase);
    }

    private void ReturnUnauthorizedResult(AuthorizationFilterContext context)
    {
        context.HttpContext.Response.Headers["WWW-Authenticate"] = $"Basic realm=\"{_realm}\"";
        context.Result = new UnauthorizedResult();
    }
}