using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using MediatR;
using Afonya.Api.Logic.Management.Queries.Authenticate;

namespace Afonya.Web.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{

    [TempData]
    public string ErrorMessage { get; set; }
    public string ReturnUrl { get; set; }
    [BindProperty, Required]
    public string Username { get; set; }
    [BindProperty, DataType(DataType.Password)]
    public string Password { get; set; }

    private readonly IMediator _mediator;

    public LoginModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void OnGet(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl = returnUrl ?? Url.Content("~/");

        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");

        if (ModelState.IsValid)
        {
            var autResult = await _mediator.Send(new AuthenticateCommand { Username = Username, Password = Password });

            if (autResult != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, autResult.Name),
                    new Claim(ClaimTypes.Sid, autResult.Id),
                    new Claim(ClaimTypes.Role, autResult.IsAdmin ? "Admin": "User")
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return Redirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Ошибка входа.");
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }
}