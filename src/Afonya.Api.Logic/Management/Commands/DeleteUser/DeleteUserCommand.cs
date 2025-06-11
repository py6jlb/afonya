using MediatR;

namespace Afonya.Api.Logic.Management.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public string Id { get; set; }
}