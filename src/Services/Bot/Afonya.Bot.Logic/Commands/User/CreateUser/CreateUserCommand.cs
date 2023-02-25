using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Commands.User.CreateUser;

public class CreateUserCommand : IRequest<UserDto>
{
    public UserDto NewUser { get; set; }
}