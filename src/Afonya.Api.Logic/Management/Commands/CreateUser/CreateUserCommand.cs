using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Management.Commands.CreateUser;

public class CreateUserCommand : IRequest<UserDto>
{
    public string Login { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }
}