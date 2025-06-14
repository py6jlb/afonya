using Afonya.Api.Logic.Categories.Queries.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Contracts;

namespace Afonya.Web.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMediator _mediator;


    public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public IEnumerable<CategoryDto> Categories { get; set; }

    public async Task OnGetAsync()
    {
        var data = await _mediator.Send(new GetCategoriesQuery { All = true });
        Categories = data;
    }
}
