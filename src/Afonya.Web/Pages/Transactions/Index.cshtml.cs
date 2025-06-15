using System.ComponentModel.DataAnnotations;
using Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

    [BindProperty, DataType("month")]
    public DateTime Date { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Month { get; set; }
    [BindProperty(SupportsGet = true)]
    public int Year { get; set; }

    public IEnumerable<MoneyTransactionDto>? Transactions { get; set; }

    public async Task OnGetAsync()
    {
        if (Month == 0)
        {
            Month = DateTime.Now.Month;
        }

        if (Year == 0)
        {
            Year = DateTime.Now.Year;
        }

        Date = new DateTime(Year, Month, 1, 0, 0, 0);

        Transactions = await _mediator.Send(new GetMoneyTransactionsQuery
        {
            Month = Month,
            Year = Year,
            User = null,
            Category = null
        });
    }

    public IActionResult OnPost()
    {
        var month = Date.Month;
        var year = Date.Year;
        return RedirectToPage($"/Transactions/Index", new { month = month, year = year });
    }
}
