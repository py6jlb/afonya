using Afonya.Api.Logic.Categories.Commands.AddCategory;
using Afonya.Api.Logic.Categories.Commands.DeleteCategory;
using Afonya.Api.Logic.Categories.Commands.UpdateCategory;
using Afonya.Api.Logic.Categories.Queries.GetCategories;
using Afonya.Web.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Afonya.Web.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IReadOnlyCollection<CategoryDto>> Get([FromQuery, SwaggerParameter("Включая неактивные")] bool all = false)
    {
        var data = await _mediator.Send(new GetCategoriesQuery { OnlyActive = all });
        return data;
    }

    [ForAdmin]
    [HttpPost]
    public async Task<CategoryDto> Post(CategoryDto category)
    {
        var data = await _mediator.Send(new AddCategoryCommand { NewCategory = category });
        return data;
    }

    [ForAdmin]
    [HttpPut]
    public async Task<CategoryDto> Put(CategoryDto category)
    {
        var data = await _mediator.Send(new UpdateCategoryCommand { Category = category });
        return data;
    }

    [ForAdmin]
    [HttpDelete]
    public async Task<bool> Delete(string id)
    {
        var data = await _mediator.Send(new DeleteCategoryCommand { Id = id });
        return data;
    }
}
