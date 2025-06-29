using Afonya.Api.Logic.Categories.Queries.GetCategories;
using Afonya.Api.Logic.MoneyTransaction.Commands.UpdateMoneyTransaction;
using Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Contracts;

namespace Afonya.Web.Pages.Transactions
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

        public IEnumerable<SelectListItem> Categories { get; set; }

        [BindProperty]
        public MoneyTransactionDto Transaction { get; set; }

        public async Task OnGetAsync()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery { OnlyActive = false });
            Categories = categories.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = $"{x.Icon}{x.HumanName}"
            });

            var transaction = await _mediator.Send(new GetMoneyTransactionQuery { Id = Id })
                ?? throw new Exception("Транзакция не найдена");
            Transaction = transaction;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var request = new UpdateMoneyTransactionCommand
            {
                Id = Transaction.Id,
                CategoryId = Transaction.CategoryId,
                FromUserName = User.Identity.Name,
                Sign = Transaction.Sign,
                TransactionDate = Transaction.TransactionDate,
                Value = Transaction.Value,
                Note = Transaction.Note
            };

            await _mediator.Send(request);
            return RedirectToPage("/Transactions/Index");
        }
    }
}
