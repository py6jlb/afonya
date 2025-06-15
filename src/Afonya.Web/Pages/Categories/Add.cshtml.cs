using Afonya.Api.Logic.Categories.Commands.AddCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Contracts;

namespace Afonya.Web.Pages.Categories
{
    public class AddModel : PageModel
    {
        private readonly ILogger<AddModel> _logger;
        private readonly IMediator _mediator;

        public AddModel(ILogger<AddModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [BindProperty]
        public string Code { get; set; }
        [BindProperty]
        public string HumanName { get; set; }
        [BindProperty]
        public string Icon { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var request = new AddCategoryCommand
            {
                NewCategory = new CategoryDto
                {
                    HumanName = HumanName,
                    Icon = Icon,
                    IsActive = true,
                    Name = Code
                }
            };
            await _mediator.Send(request);
            return RedirectToPage("/Categories/Index");
        }
    }
}
