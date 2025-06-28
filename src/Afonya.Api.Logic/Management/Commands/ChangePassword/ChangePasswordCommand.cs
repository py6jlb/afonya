using MediatR;

namespace Afonya.Api.Logic.Management.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<bool>
{
    public string Id { get; set; }
    public string Password { get; set; }
}
