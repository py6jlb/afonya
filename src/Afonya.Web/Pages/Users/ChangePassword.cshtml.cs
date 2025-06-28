using Afonya.Api.Logic.Management.Commands.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Afonya.Web.Pages.Users
{
    public class ChangePasswordModel : PageModel
    {
        private readonly ILogger<ChangePasswordModel> _logger;
        private readonly IMediator _mediator;

        public ChangePasswordModel(ILogger<ChangePasswordModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var request = new ChangePasswordCommand
            {
                Id = Id,
                Password = NewPassword,
            };

            await _mediator.Send(request);
            return RedirectToPage("/Users/Index");
        }
    }
}
