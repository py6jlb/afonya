using Afonya.Api.Logic.Categories.Commands.UpdateCategory;
using Afonya.Api.Logic.Categories.Queries.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        var data = await _mediator.Send(new GetCategoriesQuery { OnlyActive = false });
        Categories = data;
    }

    public async Task<IActionResult> OnPostToggle(string id, string icon, string name, string humanName, string isActive)
    {
        var request = new UpdateCategoryCommand
        {
            Category = new CategoryDto
            {
                Id = id,
                HumanName = humanName,
                Icon = icon,
                IsActive = isActive != "1",
                Name = name
            }
        };
        await _mediator.Send(request);
        return RedirectToPage("/Categories/Index");
    }
}
