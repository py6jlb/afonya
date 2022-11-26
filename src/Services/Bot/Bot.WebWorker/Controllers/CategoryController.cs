using Bot.Host.BasicAuth;
using Bot.Interfaces.Dto;
using Bot.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Bot.Host.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;
    private readonly ICategoryService _categoryService;

    public CategoryController(ILogger<CategoryController> logger, 
        ICategoryService categoryService)
    {
        _logger = logger;
        _categoryService = categoryService;
    }
    
    [BasicAuth("Bot")]
    public ActionResult<IEnumerable<CategoryDto>> Get([FromQuery, SwaggerParameter("Начало периода")]bool all = true)
    {
        var data = _categoryService.Get(!all);
        return Ok(data);
    }
}