using Afonya.Api.Logic.Management.Commands.DeleteUser;
using Afonya.Api.Logic.Management.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Contracts;

namespace Afonya.Web.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly ILogger<DeleteModel> _logger;
        private readonly IMediator _mediator;

        public DeleteModel(ILogger<DeleteModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public UserDto User { get; set; }

        public async Task OnGetAsync()
        {
            var user = await _mediator.Send(new GetUserByIdQuery { UserId = Id })
                ?? throw new Exception("Пользователь  не найдена");
            User = user;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var command = new DeleteUserCommand
            {
                Id = Id
            };
            await _mediator.Send(command);
            return RedirectToPage("/Users/Index");
        }

    }
}
