using Afonya.Api.Logic.MoneyTransaction.Commands.DeleteMoneyTransaction;
using Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransaction;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Contracts;

namespace Afonya.Web.Pages.Transactions
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

        public MoneyTransactionDto Transaction { get; set; }

        public async Task OnGetAsync()
        {
            var transaction = await _mediator.Send(new GetMoneyTransactionQuery { Id = Id })
                ?? throw new Exception("Транзакция не найдена");
            Transaction = transaction;
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var command = new DeleteMoneyTransactionCommand
            {
                Id = Id
            };
            await _mediator.Send(command);
            return RedirectToPage("/Transactions/Index");
        }
    }
}
