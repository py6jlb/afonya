using Afonya.Api.Logic.Management.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Afonya.Web.Pages.Users
{
    public class AddUserModel : PageModel
    {

        private readonly ILogger<AddUserModel> _logger;
        private readonly IMediator _mediator;

        public AddUserModel(ILogger<AddUserModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [BindProperty]
        public string Login { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public bool IsAdmin { get; set; } = false;

        public async Task<IActionResult> OnPostAsync()
        {
            var command = new CreateUserCommand
            {
                Login = Login,
                Password = Password,
                IsAdmin = IsAdmin
            };
            await _mediator.Send(command);
            return RedirectToPage("/Users/Index");
        }
    }
}
