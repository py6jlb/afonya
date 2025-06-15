using System.ComponentModel.DataAnnotations;
using Afonya.ApiLogic.Statistics.Queries.GetStatistics;
using Afonya.Shared.Interfaces.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Afonya.Web.Pages.Statistics
{
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
        public StatisticsDto Statistics { get; set; }

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

            var request = new GetStatisticsQuery
            {
                Month = Month,
                Year = Year
            };
            var r = await _mediator.Send(request);
            Statistics = r;
        }

        public IActionResult OnPost()
        {
            var month = Date.Month;
            var year = Date.Year;
            return RedirectToPage($"/Statistics/Index", new { month = month, year = year });
        }
    }
}
