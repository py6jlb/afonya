using Afonya.Bot.Logic.Api.Management.Commands.CreateUser;
using Afonya.Bot.Logic.Api.Management.Commands.DeleteUser;
using Afonya.Bot.Logic.Api.Management.Queries.GetUser;
using Afonya.Bot.WebWorker.Auth;
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

        [BasicAuthAdmin]
        [HttpGet]
        public async Task<UserDto?> Get(string? userName)
        {
            var data = await _mediator.Send(new GetUserQuery{ UserName = userName });
            return data;
        }
    
        [BasicAuthAdmin]
        [HttpPost]
        public async Task<UserDto> Post(UserDto user)
        {
            var data = await _mediator.Send(new CreateUserCommand { NewUser = user });
            return data;
        }
    
        [BasicAuthAdmin]
        [HttpDelete]
        public async Task<bool> Delete(string id)
        {
            var data = await _mediator.Send(new DeleteUserCommand { Id = id });
            return data;
        }
    }
}
