using System.ComponentModel.DataAnnotations;
using Afonya.Api.Logic.Categories.Queries.GetCategories;
using Afonya.Api.Logic.MoneyTransaction.Queries.GetMoneyTransactions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public string? Category { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Month { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Year { get; set; }

    public IEnumerable<MoneyTransactionDto>? Transactions { get; set; }
    public IEnumerable<SelectListItem>? Categories { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (Month == 0 || Year == 0)
        {
            Month = HttpContext.Session.GetInt32("Month") ?? DateTime.Now.Month;
            HttpContext.Session.SetInt32("Month", Month);
            Year = HttpContext.Session.GetInt32("Year") ?? DateTime.Now.Year;
            HttpContext.Session.SetInt32("Year", Year);

            Category ??= HttpContext.Session.GetString("Category");
            if (Category != null)
            {
                HttpContext.Session.SetString("Category", Category);
            }

            return RedirectToPage($"/Transactions/Index", new
            {
                month = Month,
                year = Year,
                category = Category
            });
        }

        Date = new DateTime(Year, Month, 1, 0, 0, 0);
        Transactions = await _mediator.Send(new GetMoneyTransactionsQuery
        {
            Month = Date.Month,
            Year = Date.Year,
            User = null,
            Category = Category
        });

        var categories = await _mediator.Send(new GetCategoriesQuery { OnlyActive = false });
        var c = new List<SelectListItem>
        {
            new() {
                Value = "",
                Text = "Все категории"
            }
        };
        c.AddRange(categories.Select(x => new SelectListItem
        {
            Value = x.Id,
            Text = $"{x.Icon}{x.HumanName}"
        }));

        Categories = c;

        return Page();
    }

    public IActionResult OnPost()
    {
        var month = Date.Month;
        var year = Date.Year;

        HttpContext.Session.SetInt32("Year", year);
        HttpContext.Session.SetInt32("Month", month);
        if (Category != null)
        {
            HttpContext.Session.SetString("Category", Category);
        }

        return RedirectToPage($"/Transactions/Index", new { month, year, category = Category });
    }
}
