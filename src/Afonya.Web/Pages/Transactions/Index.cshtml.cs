using Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Contracts;

namespace Afonya.Web.Pages.Transactions;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;

    public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public int? Month { get; set; }
    public int? Year { get; set; }
    public string? AppUser { get; set; }
    public string? Category { get; set; }

    public IEnumerable<MoneyTransactionDto>? Transactions { get; set; }

    public async Task OnGetAsync()
    {

        Transactions = await _mediator.Send(new GetMoneyTransactionsQuery
        {
            Month = Month,
            Year = Year,
            User = AppUser,
            Category = Category
        });
    }
}
