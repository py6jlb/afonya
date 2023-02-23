using Afonya.Bot.Logic.Commands.User.CreateUser;
using Afonya.Bot.Logic.Commands.User.DeleteUser;
using Afonya.Bot.Logic.Queries.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts;

namespace Afonya.Bot.WebWorker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<UserDto?> Get(string? userName)
        {
            var data = await _mediator.Send(new GetUserQuery{ UserName = userName });
            return data;
        }
    
        [HttpPost]
        public async Task<UserDto> Post(UserDto user)
        {
            var data = await _mediator.Send(new CreateUserCommand { NewUser = user });
            return data;
        }
    
        [HttpDelete]
        public async Task<bool> Delete(string id)
        {
            var data = await _mediator.Send(new DeleteUserCommand { Id = id });
            return data;
        }
    }
}
