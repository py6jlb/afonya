using MediatR;

namespace Afonya.Bot.Logic.Api.Management.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<bool>
{
    public string Id { get; set; }
}