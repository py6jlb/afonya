using MediatR;
using Shared.Contracts;

namespace Afonya.Bot.Logic.Api.Management.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<UserDto?>
{
    public string Id { get; set; }
    public string Password { get; set; }
}
