using MediatR;

namespace Afonya.Api.Logic.Management.Commands.EditUser;

public class EditUserCommand : IRequest<bool>
{
    public string Id { get; set; }
    public string Login { get; set; }
    public bool IsAdmin { get; set; }
}
