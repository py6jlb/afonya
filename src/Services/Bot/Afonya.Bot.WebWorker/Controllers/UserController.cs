using Afonya.Bot.Logic.Api.Management.Commands.ChangePassword;
using Afonya.Bot.Logic.Api.Management.Commands.CreateUser;
using Afonya.Bot.Logic.Api.Management.Commands.DeleteUser;
using Afonya.Bot.Logic.Api.Management.Queries.Authenticate;
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

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto loginData)
        {
            var autResult = await _mediator.Send(new AuthenticateCommand { Username = loginData.Login, Password = loginData.Password });

            if (autResult == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(autResult);
        }


        [BasicAuthAdmin]
        [HttpGet]
        public async Task<UserDto?> Get(string? userName)
        {
            var data = await _mediator.Send(new GetUserQuery { UserName = userName });
            return data;
        }

        [BasicAuthAdmin]
        [HttpPost]
        public async Task<UserDto?> Post([FromBody] LoginDto user)
        {
            var data = await _mediator.Send(new CreateUserCommand { Login = user.Login, Password = user.Password });
            return data;
        }

        [BasicAuthAdmin]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ChangePasswordDto data)
        {
            var result = await _mediator.Send(new ChangePasswordCommand { Id = data.Id, Password = data.Password });
            if (result == null)
                return BadRequest(new { message = "Error on change password" });
            return Ok(result);
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
