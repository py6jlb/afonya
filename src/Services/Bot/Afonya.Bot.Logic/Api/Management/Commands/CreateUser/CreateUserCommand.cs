using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Commands.CreateUser;

public class CreateUserCommand : IRequest<UserDto>
{
    public UserDto NewUser { get; set; }
}