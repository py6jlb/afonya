using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Commands.CreateUser;

public class CreateUserCommand : IRequest<UserDto>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
}