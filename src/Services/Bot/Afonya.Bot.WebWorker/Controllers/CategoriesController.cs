using Afonya.Bot.Logic.Commands.Categories.AddCategory;
using Afonya.Bot.Logic.Commands.Categories.DeleteCategory;
using Afonya.Bot.Logic.Commands.Categories.UpdateCategory;
using Afonya.Bot.Logic.Queries.GetCategories;
using Afonya.Bot.WebWorker.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace Afonya.Bot.WebWorker.Controllers
{
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
        public async Task<IReadOnlyCollection<CategoryDto>> Get([FromQuery, SwaggerParameter("Включая неактивные")]bool all = false)
        {
            var data = await _mediator.Send(new GetCategoriesQuery{All = all});
            return data;
        }
    
        [BasicAuthAdmin]
        [HttpPost]
        public async Task<CategoryDto> Post(CategoryDto category)
        {
            var data = await _mediator.Send(new AddCategoryCommand{NewCategory = category});
            return data;
        }
        
        [BasicAuthAdmin]
        [HttpPut]
        public async Task<CategoryDto> Put(CategoryDto category)
        {
            var data = await _mediator.Send(new UpdateCategoryCommand{ Category = category});
            return data;
        }
    
        [BasicAuthAdmin]
        [HttpDelete]
        public async Task<bool> Delete(string id)
        {
            var data = await _mediator.Send(new DeleteCategoryCommand {Id = id});
            return data;
        }
    }
}
