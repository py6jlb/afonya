using Afonya.ApiLogic.Statistics.Queries.GetStatistics;
using Afonya.Shared.Interfaces.Dto;
using MediatR;
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

        public DateTime Date { get; set; } = DateTime.Now;
        public StatisticsDto Statistics { get; set; }

        public async Task OnGetAsync()
        {
            var request = new GetStatisticsQuery
            {
                Month = Date.Month,
                Year = Date.Year
            };
            var r = await _mediator.Send(request);
            Statistics = r;
        }
    }
}
