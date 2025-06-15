using Afonya.Api.Logic.Categories.Queries.GetCategories;
using Afonya.Api.Logic.MoneyTransaction.Commands.CreateMoneyTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Contracts;

namespace Afonya.Web.Pages.Transactions
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

        public IEnumerable<SelectListItem> Categories { get; set; }

        [BindProperty]
        public string Sign { get; set; } = "-";
        [BindProperty]
        public float Value { get; set; } = 0f;
        [BindProperty]
        public DateTime Date { get; set; } = DateTime.Now;
        [BindProperty(Name = "category")]
        public string? CategoryId { get; set; }

        public async Task OnGetAsync()
        {
            var data = await _mediator.Send(new GetCategoriesQuery { OnlyActive = false });
            Categories = data.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = $"{x.Icon}{x.HumanName}"
            });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var command = new CreateMoneyTransactionCommand
            {
                Sign = Sign,
                Value = Value,
                Date = Date,
                CategoryId = CategoryId,
                FromUsername = User.Identity.Name
            };
            await _mediator.Send(command);
            return RedirectToPage("/Transactions/Index");
        }
    }
}
