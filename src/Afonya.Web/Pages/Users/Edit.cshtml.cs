using Afonya.Api.Logic.Management.Commands.EditUser;
using Afonya.Api.Logic.Management.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Contracts;

namespace Afonya.Web.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly ILogger<EditModel> _logger;
        private readonly IMediator _mediator;

        public EditModel(ILogger<EditModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }


        [BindProperty]
        public UserDto AppUser { get; set; }
        
        public async Task OnGetAsync()
        {
            var user = await _mediator.Send(new GetUserByIdQuery { UserId = Id })
                ?? throw new Exception("Пользователь не найден"); ;
            AppUser = user;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var request = new EditUserCommand
            {
                Id = AppUser.Id,
                Login = AppUser.Login,
                IsAdmin = AppUser.IsAdmin,
            };

            await _mediator.Send(request);
            return RedirectToPage("/Users/Index");
        }
    }
}
