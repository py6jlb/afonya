using MediatR;
using Shared.Contracts;

namespace Afonya.Api.Logic.Management.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<UserDto?>
{
    public string Id { get; set; }
    public string Password { get; set; }
}
